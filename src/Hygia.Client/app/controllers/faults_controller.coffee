define ['chaplin','models/fault','models/faults','controllers/base/controller', 'views/faults_view'], (Chaplin, Fault, Faults, Controller, FaultsView) ->
  'use strict'

  class FaultsController extends Controller

    title: 'Fault Management'

    historyURL: (params) =>
      path = @arguments.callee.name.replace 'Controller', ''
      if params.id then "#{path}/#{params.id}" else ''

    index: (params) ->      
      @faults = new Faults()
      @view = new FaultsView collection: @faults

    show: (params) ->      
      @fault = new Fault {id: params.id}, {loadDetails: true}
      @view = new FaultView model: @fault