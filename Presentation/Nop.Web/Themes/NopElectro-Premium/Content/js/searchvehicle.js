$(document).ready(function () {
    //sessionStorage.setItem("baseVehicleID", "0");
    //sessionStorage.setItem("VehicleID", "0");
    //sessionStorage.setItem("EngineID", "0");
    if ($('#hform').val() == "1") {
          
        $("#ClearSelected").show();
        $("#FindParts").hide();
    } else {
        $("#ClearSelected").hide();
        $("#FindParts").show();
    }

     
    $('#success').on('closed.bs.alert', function () {
        $.ajax({
            url: '@Url.Action("ClearCureentVehicle", "Catalog")',
            type: "GET",
        dataType: "json",
        //data: mYear,
        success: function (makes) {
            // alert('1');
            $("hform").val("0");
            $("#MakeID").attr('class', 'form-control input-sm');
            $("#ModelID").attr('class', 'form-control input-sm');
            $("#Year").attr('class', 'form-control input-sm');
            $("#ClearSelected").hide();
            $("#FindParts").show();
        },

        error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.responseText);
        }
    });
})
$("#ClearSelected").click(function (e) {
    e.preventDefault();

    //var postData = {
    //    Year: $("#Year").val(),
    //    MakeID:$("#MakeID").val(),
    //    ModelID:$("#ModelID").val(),
    //    VehicleID:$("#VehicleID").val(),
    //    EngineID:$("#Egine").val()
    //};

    $.ajax({
        url: '@Url.Action("ClearCureentVehicle", "Catalog")',
        type: "GET",
    dataType: "json",
    //data: mYear,
    success: function (makes) {
        // alert('1');
        $("hform").val("1");
        $("#MakeID").attr('class', 'form-control input-sm');
        $("#ModelID").attr('class', 'form-control input-sm');
        $("#Year").attr('class', 'form-control input-sm');
        $('#success').alert('close');
        $("#ClearSelected").hide();
        $("#FindParts").show();
    },

    error: function (xhr, ajaxOptions, thrownError) {
        alert(xhr.responseText);
    }
});

           
//alert('ajax call');
alert(postData.MakeID.toString());

$.post('@Url.Action("FindParts", "Catalog")', { formData: postData },
function () { });

});



$("#trim-dv").hide();
$("#engine-dv").hide();
$("#FindParts").prop('disabled', true);
function listMake(){
    var mYear={
        year: $("#Year").val(),
        EndYear: "0"
    };
    //addAntiForgeryToken(mYear);

    //alert(mYear.year);


    $('#MakeID').attr("disabled",false);
    $.ajax({
        url: '@Url.Action("ListMake", "Catalog")',
        type: "GET",
    dataType: "json",
    data: mYear,
    success: function (makes) {
        // alert('1');
        // alert(makes[0].text);
        $("#MakeID").html(""); // clear before appending new list
        //alert('2');
        dataobjects=$.map(makes,function(el) {return el});
        //alert(dataobjects.length);
        //alert(dataobjects.length);
        //$.each(dataobjects, function (key, value) {
        //    $('#MakeID select').append('<option value=' + value.id + '>' + value.text + '</option>');
        //}
        //);

        $("#MakeID").select2({data:dataobjects,
            placeholder:"Select Make"

        });

    },

    error: function (xhr, ajaxOptions, thrownError) {
        alert(xhr.responseText);
    }
});
}
$("#Year").change(function(){
    listMake();
    $("#VehicleID").html("");
    $("#ModelID").html("");
    $("#EngineID").html("");
    disableFind();
});



$("#MakeID").change(function(){
    listModel();
    $("#VehicleID").html("");
    $("#EngineID").html("");
    disableFind();
});

function listModel(){
    var mYear={
        year: $("#Year").val(),
        EndYear: "0",
        MakeID: $("#MakeID").val()

    };
    //addAntiForgeryToken(mYear);

    //alert(mYear.year);


    $('#ModelID').attr("disabled",false);

    $.ajax({
        url: '@Url.Action("ListModel", "Catalog")',
        type: "GET",
    dataType: "json",
    data: mYear,
    success: function (models) {
        //alert('1');
        $("#ModelID").html(""); // clear before appending new list
        //alert('2');
        dataobjects=$.map(models,function(el) {return el});
        //alert(dataobjects.length);
        $("#ModelID").select2({data:dataobjects,
            placeholder:"Select Model"});

    }
});
}

function listTrim(){
    var mYear={
        year: $("#Year").val(),
        EndYear:"0",
        MakeID: $("#MakeID").val(),
        ModelID: $('#ModelID').val()

    };
    //addAntiForgeryToken(mYear);

    //alert(mYear.year);
    //if(!$('#yearrange').is(":checked")){

    $("#trim-dv").show();
    $('#VehicleID').attr("disabled",false);
    // alert(mYear.ModelID);
    $.ajax({
        url: '@Url.Action("ListTrim", "Catalog")',
        type: "GET",
    dataType: "json",
    data: mYear,
    success: function (models) {
        //alert('1');
        $("#VehicleID").html(""); // clear before appending new list
        //alert('2');
        dataobjects=$.map(models,function(el) {return el});
        //alert(dataobjects.length);
        $("#VehicleID").select2({data:dataobjects,
            placeholder:"Select Trinm"});

    }
});
}
$("#ModelID").change(function(){
    listTrim();
    $("#EngineID").html("");
    disableFind();

});

function listEngine(){
    var mYear={

        VehicelID: $('#VehicleID').val()

    };
    //addAntiForgeryToken(mYear);

    //alert(mYear.year);
    //if(!$('#yearrange').is(":checked")){

    $("#engine-dv").show();

    $('#EngineID').attr("disabled",false);
    //alert(mYear.VehicleID);
    $.ajax({
        url: '@Url.Action("ListEngine", "Catalog")',
        type: "GET",
    dataType: "json",
    data: mYear,
    success: function (models) {
        //alert('1');
        $("#EngineID").html(""); // clear before appending new list
        //alert('2');
        dataobjects=$.map(models,function(el) {return el});
        //alert(dataobjects.length);
        $("#EngineID").select2({data:dataobjects,
            placeholder:"Select Engine...."});

    }
});
}

$("#VehicleID").change(function(){

    listEngine();
    disableFind();

});

});