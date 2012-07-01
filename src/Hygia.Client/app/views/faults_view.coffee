define ['chaplin','views/base/collection_view','views/fault_view','text!templates/faults.hbs'], (Chaplin, CollectionView, FaultView, template) ->
  'use strict'

  class FaultsView extends CollectionView

    # Save the template string in a prototype property.
    # This is overwritten with the compiled template function.
    # In the end you might want to used precompiled templates.
    template: template
    template = null

    tagName: 'div' # This is not directly a list but contains a list
    id: 'faults'

    # Automatically append to the DOM on render
    container: '#main'

    # Append the item views to this element
    listSelector: 'ol'
    # Fallback content selector
    fallbackSelector: '.fallback'
    # Loading indicator selector
    loadingSelector: '.loading'

    initialize: ->
      super # Will render the list itself and all items      

    # The most important method a class derived from CollectionView
    # must overwrite.
    getView: (item) ->
      # Instantiate an item view
      new FaultView model: item

    render: ->
      console.log 'render faults'
      super    