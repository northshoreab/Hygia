define ['models/base/model'], (Model) ->
  'use strict'

  class Navigation extends Model

    defaults:
      projectName: 'WatchR',
      links: ['Home','About','Contact','Faults']
