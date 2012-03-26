(function() {

  define(['login'], function(Login) {
    return describe('Login', function() {
      return describe('login method', function() {
        var originalLocation;
        originalLocation = window.location;
        afterEach(function() {
          return window.location = originalLocation;
        });
        return it('should redirect to login page', function() {
          window.location = '#users/enumerate';
          Login.login();
          return expect(window.location.hash).toBe('#login');
        });
      });
    });
  });

}).call(this);
