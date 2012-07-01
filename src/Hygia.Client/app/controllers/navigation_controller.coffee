define ['chaplin','controllers/base/controller','models/navigation','views/navigation_view'], (Chaplin, Controller, Navigation, NavigationView) ->
  
  class NavigationController extends Controller
    initialize: ->
      super
      #console.debug 'NavigationController#initialize'
      @navigation = new Navigation()
      @view = new NavigationView model: @navigation		
      return