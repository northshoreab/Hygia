define ['chaplin','models/base/collection', 'models/fault'], (Chaplin, Collection, Fault) ->
  'use strict'
  
  class Faults extends Collection
    model: Fault
    url: '/api/faults'

    initialize: ->
      super
      @fetch()

