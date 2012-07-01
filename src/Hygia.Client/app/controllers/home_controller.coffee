define ['chaplin','models/home','views/home_view'], (Chaplin, Home, HomeView) ->
  'use strict'

  class HomeController extends Chaplin.Controller

    title: 'WatchR'

    historyURL: (params) ->
      ''

    show: (params) ->      
      @model = new Home()
      @view = new HomeView model: @model
