define ['chaplin','models/user','views/hello_world_view'], (Chaplin, User, HelloWorldView) ->
  'use strict'
	
  class UserController extends Chaplin.Controller
  	
  	title: 'User information'
  	
  	historyURL: (params) ->
      'user'

    show: (params) ->
      @model = new User()
      @view = new HelloWorldView model: @model