var everything = @JavaScriptConvert.SerializeObject(Model.jsonQuestions );

    $('#consume').text(everything);
    $("#target").click(function () {

        $('#consume').text(everything);
    });