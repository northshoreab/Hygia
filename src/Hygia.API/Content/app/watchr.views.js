
WatchR.ItemView = Backbone.Marionette.ItemView;
WatchR.CollectionView = Backbone.Marionette.CollectionView;

/*
Backbone.Marionette.ItemView.prototype.renderTemplate = function(template, data){
  var html = template.tmpl(data);
  return html;
};
*/

Backbone.Marionette.TemplateCache.loadTemplate = function(templateId, callback){
	var tmpId = templateId.replace("#", "");
  	var url = "/content/templates/" + tmpId + ".html";
  	var promise = $.trafficCop(url);
  	promise.done(function(template){
    	callback.call(this, template);
  	});
}