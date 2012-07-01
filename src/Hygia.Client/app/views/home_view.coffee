define ['views/base/view','text!templates/home.hbs'], (View, template) ->
  'use strict'

  class HomeView extends View

    # Save the template string in a prototype property.
    # This is overwritten with the compiled template function.
    # In the end you might want to used precompiled templates.
    template: template
    template = null

    className: ''

    # Automatically append to the DOM on render
    container: '#main'
    # Automatically render after initialize
    autoRender: true
