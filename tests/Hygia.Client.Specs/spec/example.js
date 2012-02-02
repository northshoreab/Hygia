describe("Hygia", function() {
  var counter = 0;	

  it("should be an awesome project!", function() {
    counter = counter + 2;   // counter was 0 before
    expect(counter).toEqual(2)    
  });
});