// Set the require.js configuration for your application.
require.config({
  // Initialize the application with the main application file
  deps: ["main"],

  paths: {
    // JavaScript folders
    libs: "/content/assets/js/libs",
    plugins: "/content/assets/js/plugins",

    // Libraries
    jquery: "/content/assets/js/libs/jquery",
    underscore: "/content/assets/js/libs/underscore",
    backbone: "/content/assets/js/libs/backbone",

    // Shim Plugin
    use: "/content/assets/js/plugins/use"
  },

  use: {
    backbone: {
      deps: ["use!underscore", "jquery"],
      attach: "Backbone"
    },

    underscore: {
      attach: "_"
    }
  }
});
