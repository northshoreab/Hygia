(function(WatchR, Backbone, $){
  
	var Layout = Backbone.Marionette.Layout.extend({
		template: "#layout-template",

		regions: {      
			main: "#main"
		},
    
		events: {
		},

		initialize: function(){
		},
	});

  WatchR.addInitializer(function(){    
    WatchR.layout = new Layout();
    var layoutRender = WatchR.layout.render();
    $("body").prepend(WatchR.layout.el);
    
    layoutRender.done(function(){
      Backbone.history.start();
    });
  });

})(WatchR, Backbone, $);