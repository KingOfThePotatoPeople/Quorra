$(document).ready(function () {
  // Color Picker Tool Js
  const themeSwitchers = document.querySelectorAll('span');
  const dynamicInputs = document.querySelectorAll('input.input-color-picker');
  
  const handleThemeUpdate = (cssVars) => {
    const root = document.querySelector(':root');
    const keys = Object.keys(cssVars);
    keys.forEach(key => {
      root.style.setProperty(key, cssVars[key]);
    });
  }
   const handleThemeUpdate1 = (cssVars) => {
    const root = document.querySelector(':root');
    const keys = Object.keys(jsVars);
    keys.forEach(key => {
      root.style.setProperty(key, jsVars[key]);
    });
  }
  
  themeSwitchers.forEach((item) => {
    item.addEventListener('click', (e) => {
      const bgColor = e.target.getAttribute('data-bg-color')
      const color = e.target.getAttribute('data-color')
      const transparentcolor = e.target.getAttribute('data-transparentcolor')
      const primarydark = e.target.getAttribute('data-primary-dark')
      handleThemeUpdate({
        '--primary-bg-color': bgColor,
        '--primary-color': color,
        '--primary-transparentcolor': transparentcolor,
        '--primary-primary-dark': primarydark,
      });
      
      console.log(bgColor, color, primarydark)
      $("input.input-color-picker[data-id='color']").val(color)
      $("input.input-color-picker[data-id='bg-color']").val(bgColor)
      $("input.input-color-picker[data-id='transparentcolor']").val(transparentcolor)
      $("input.input-color-picker[data-id='primary-dark']").val(primarydark)
    });
  });
  
  dynamicInputs.forEach((item) => {
    item.addEventListener('input', (e) => {
      const cssPropName = `--primary-${e.target.getAttribute('data-id')}`;
      console.log(cssPropName)
      handleThemeUpdate({
        [cssPropName]: e.target.value
      });
    });
  });
});

 jQuery(document).ready(function($){
 $('.red-btn').on({
     'click': function(){
         $('#change-image').attr('src','../assets/img/brand/logo-red.png');
         $('#change-logo-dark').attr('src','../assets/img/brand/logo-red-dark.png');
         $('#img-change').attr('src','../assets/img/svgicons/offer-red.svg');
     }
 });
 
$('.purple-btn').on({
     'click': function(){
        $('#change-image').attr('src','../assets/img/brand/logo-purple.png');
		 $('#change-logo-dark').attr('src','../assets/img/brand/logo-purple-dark.png');
		$('#img-change').attr('src','../assets/img/svgicons/offer-purple.svg');
		$('#change-js').attr('src','../assets/js/index-purple.js');
     }
 });
 
$('.green-btn').on({
     'click': function(){
         $('#change-image').attr('src','../assets/img/brand/logo-green.png');
		  $('#change-logo-dark').attr('src','../assets/img/brand/logo-green-dark.png');
		 $('#img-change').attr('src','../assets/img/svgicons/offer-green.svg');
     }
 });
 
$('.pink-btn').on({
     'click': function(){
         $('#change-image').attr('src','../assets/img/brand/logo-pink.png');
		  $('#change-logo-dark').attr('src','../assets/img/brand/logo-pink-dark.png');
		 $('#img-change').attr('src','../assets/img/svgicons/offer-pink.svg');
     }
 });
 $('.orange-btn').on({
     'click': function(){
         $('#change-image').attr('src','../assets/img/brand/logo-orange.png');
		  $('#change-logo-dark').attr('src','../assets/img/brand/logo-orange-dark.png');
		 $('#img-change').attr('src','../assets/img/svgicons/offer-orange.svg');
     }
 });
});