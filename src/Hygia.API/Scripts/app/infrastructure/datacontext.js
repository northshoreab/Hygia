﻿define('datacontext',
    ['jquery', 'underscore', 'ko', 'model', 'model.mapper', 'dataservice', 'config', 'utils'],
    function ($, _, ko, model, modelmapper, dataservice, config, utils) {
        var logger = config.logger,
            getCurrentEnvironmentId = function () {
                return config.selectedEnvironment().id();
            },
        //            getCurrentUserId = function() {
        //                return config.currentUser().id();
        //            },
            itemsToArray = function (items, observableArray, filter, sortFunction) {
                // Maps the memo to an observableArray, 
                // then returns the observableArray
                if (!observableArray) return;

                // Create an array from the memo object
                var underlyingArray = utils.mapMemoToArray(items);

                if (filter) {
                    underlyingArray = _.filter(underlyingArray, function (o) {
                        var match = filter.predicate(filter, o);
                        return match;
                    });
                }
                if (sortFunction) {
                    underlyingArray.sort(sortFunction);
                }
                //logger.info('Fetched, filtered and sorted ' + underlyingArray.length + ' records');
                observableArray(underlyingArray);
            },
            mapToContext = function (dtoList, items, results, mapper, filter, sortFunction) {
                // Loop through the raw dto list and populate a dictionary of the items

                items = _.reduce(dtoList, function (memo, dto) {
                    var id = mapper.getDtoId(dto);
                    var existingItem = items[id];
                    memo[id] = mapper.fromDto(dto, existingItem);
                    return memo;
                }, {});
                
                itemsToArray(items, results, filter, sortFunction);
                //logger.success('received with ' + dtoList.length + ' elements');
                return items; // must return these
            },
            
            EntitySet = function (getFunction, mapper, nullo, updateFunction) {
                var items = {},
                // returns the model item produced by merging dto into context
                    mapDtoToContext = function (dto) {
                        var id = mapper.getDtoId(dto);
                        var existingItem = items[id];
                        items[id] = mapper.fromDto(dto, existingItem);
                        return items[id];
                    },
                    add = function (newObj) {
                        items[newObj.id()] = newObj;
                    },
                    removeById = function (id) {
                        delete items[id];
                    },
                    getLocalById = function (id) {
                        // This is the only place we set to NULLO
                        return !!id && !!items[id] ? items[id] : nullo;
                    },
                    getAllLocal = function () {
                        return utils.mapMemoToArray(items);
                    },
                    getData = function (options) {
                        return $.Deferred(function (def) {
                            var results = options && options.results,
                                sortFunction = options && options.sortFunction,
                                filter = options && options.filter,
                                forceRefresh = options && options.forceRefresh,
                                param = options && options.param,
                                getFunctionOverride = options && options.getFunctionOverride;

                            getFunction = getFunctionOverride || getFunction;

                            // If the internal items object doesnt exist, 
                            // or it exists but has no properties, 
                            // or we force a refresh
                            if (forceRefresh || !items || !utils.hasProperties(items)) {
                                getFunction({
                                    success: function (dtoList) {
                                        items = mapToContext(dtoList.results, items, results, mapper, filter, sortFunction);
                                        def.resolve(results);
                                    },
                                    error: function (response) {
                                        logger.error(config.toasts.errorGettingData);
                                        def.reject();
                                    }
                                }, param);
                            } else {
                                itemsToArray(items, results, filter, sortFunction);
                                def.resolve(results);
                            }
                        }).promise();
                    },
                    updateData = function (entity, callbacks) {

                        var entityJson = ko.toJSON(entity);

                        return $.Deferred(function (def) {
                            if (!updateFunction) {
                                logger.error('updateData method not implemented');
                                if (callbacks && callbacks.error) {
                                    callbacks.error();
                                }
                                def.reject();
                                return;
                            }

                            updateFunction({
                                success: function (response) {
                                    logger.success(config.toasts.savedData);
                                    entity.dirtyFlag().reset();
                                    if (callbacks && callbacks.success) {
                                        callbacks.success();
                                    }
                                    def.resolve(response);
                                },
                                error: function (response) {
                                    logger.error(config.toasts.errorSavingData);
                                    if (callbacks && callbacks.error) {
                                        callbacks.error();
                                    }
                                    def.reject(response);
                                    return;
                                }
                            }, entityJson);
                        }).promise();
                    };

                return {
                    mapDtoToContext: mapDtoToContext,
                    add: add,
                    getAllLocal: getAllLocal,
                    getLocalById: getLocalById,
                    getData: getData,
                    removeById: removeById,
                    updateData: updateData
                };
            },
        //----------------------------------
        // Repositories
        //
        // Pass: 
        //  dataservice's 'get' method
        //  model mapper
        //----------------------------------
            faults = new EntitySet(dataservice.fault.getFaults, modelmapper.fault, model.Fault.Nullo);
            environments = new EntitySet(dataservice.environment.getMyEnvironments, modelmapper.environment, model.Environment.Nullo);
            users = new EntitySet(null, modelmapper.user, model.User.Nullo);         

            environments.addData = function (envModel, callbacks) {
                var environmentModel = new model.Environment()
                        .id(envModel.id)
                        .name(envModel.name),
                    environmentModelJson = ko.toJSON(environmentModel);

                return $.Deferred(function (def) {
                    dataservice.environment.addEnvironment({
                        success: function (dto) {
                            if (!dto) {
                                if (callbacks && callbacks.error) { callbacks.error(); }
                                def.reject();
                                return;
                            }
                            var newEnv = modelmapper.environment.fromDto(dto); // Map DTO to Model
                            environments.add(newEnv); // Add to the datacontext

                            if (callbacks && callbacks.success) { callbacks.success(newEnv); }
                            def.resolve(dto);
                        },
                        error: function (response) {
                            if (callbacks && callbacks.error) { callbacks.error(); }
                            def.reject(response);
                            return;
                        }
                    }, environmentModelJson);
                }).promise();
            },
            faults.getFaults = function (callbacks) {
                return $.Deferred(function (def) {
                    var items = [];

                    dataservice.fault.getFaults({
                        success: function (dto) {
                            //faults = mapToContext(dto.results, faults, results, mapper, filter, sortFunction);
                            for (var i in dto.results) {
                                items[i] = modelmapper.fault.fromDto(dto.results[i], null);
                            }
                            callbacks.success(items);
                            def.resolve(dto);
                        },
                        error: function (response) {
                            if (callbacks && callbacks.error) {
                                callbacks.error(response);
                            }
                            def.reject(response);
                        }
                    });

                }).promise();
            },
            users.getMe = function (callbacks) {
                return $.Deferred(function (def) {
                    var user;
                    
                    dataservice.user.getMe({
                        success: function (dto, status) {
                            user = modelmapper.user.fromDto(dto, user);
                            callbacks.success(user);
                            def.resolve(dto);
                        },
                        error: function (response, status, p) {
                            if (callbacks && callbacks.error) {
                                 callbacks.error(response);
                            }
                            def.reject(response);
                        }
                    });

                }).promise();
            };

        var datacontext = {
            faults: faults,
            users: users,
            environments: environments
        };

        // We did this so we can access the datacontext during its construction
        model.setDataContext(datacontext);

        return datacontext;
    });