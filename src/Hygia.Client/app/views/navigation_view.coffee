define ['chaplin','views/base/view','text!templates/navigation.hbs','bootstrap'], (Chaplin, View, template) ->
  'use strict'

  class NavigationView extends View

    initialize: ->
        console.log Chaplin        

    # Save the template string in a prototype property.
    # This is overwritten with the compiled template function.
    # In the end you might want to used precompiled templates.
    template: template
    template = null

    className: ''

    # Automatically append to the DOM on render
    container: '#topnav'
    # Automatically render after initialize
    autoRender: true    
