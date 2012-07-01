define ['views/base/view','text!templates/fault.hbs'], (View, template) ->
  'use strict'

  class FaultView extends View

    # Save the template string in a prototype property.
    # This is overwritten with the compiled template function.
    # In the end you might want to used precompiled templates.
    template: template
    template = null
    tagName = 'li'
    className: 'fault'

