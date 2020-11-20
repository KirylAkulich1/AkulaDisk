$(document).ready(function () {
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });
});
function submitForm(formId) {
    document.getElementById(formId).submit();
  }

  $('#upload-btn').click(function(){
    $('#buttonupload').click();
});
function thisFileUpload() {
    document.getElementById("buttonupload").click();
};