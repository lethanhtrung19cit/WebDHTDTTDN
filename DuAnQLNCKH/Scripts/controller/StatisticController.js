
$(document).ready(function () {

    function Contains(text_one, text_two) {
        if (text_one.indexOf(text_two) != -1)
            return true;
    }
    $("#Search").keyup(function () {
        var searchText = $("#Search").val().toLowerCase();
        var searchYear = $("#StatisticYear").val();
        $(".Search").each(function () {
            var id = $(this).find(".nametd");
            var idYear = $(this).find(".Year").text().substr(6, 4);
            if (Contains($(id).text().toLowerCase(), searchText) && Contains(idYear, searchYear)) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        })
    })
    $("#SearchStudent").keyup(function () {
        var searchText = $("#SearchStudent").val().toLowerCase();

        $(".Searchstudent").each(function () {
            var id = $(this).find(".namestudent");
            if (!Contains($(id).text().toLowerCase(), searchText)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        })

    })
    $("#StatisticYear").change(function () {
        var searchText = $("#StatisticYear").val();

        $(".Search").each(function () {
            var id = $(this).find(".Year").text().substr(6, 4);

            if (!Contains(id, searchText)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        })
    })

    $("#buttonSearch").click(function () {


         
        //var a = $("#IdP option:selected").val();
         var a = $("#IdP option:selected").val();
        $("#IdPa").val(a);
        var dateSt = $("#DateSt").val();
        var dateEnd = $("#DateEnd").val();

        var dateEnd1 = new Date(dateEnd);
        var dateSt1 = new Date(dateSt);
        $("#DateSt1").val(dateSt);
        $("#DateEnd1").val(dateEnd);
       
        
        var IdPSearch = $("#IdP").val();
        var IdTySearch = $("#IdTy").val();
        
        $(".Search").each(function () {

            var id = $(this).find(".Year1").val();
            var IdP = $(this).find(".IdP").text();
            if (IdTySearch == "") {
                if (dateSt1 < new Date(id) && dateEnd1 > new Date(id)) {

                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            }
            else {

                if (dateSt1 < new Date(id) && dateEnd1 > new Date(id) && Number(IdP) == Number(IdPSearch)) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
            }
        })


    })
    $("#IdTy").change(function () {
        $.get("/Statistic/getTypeList", { IdTy: $("#IdTy").val() }, function (data) {
            $("#IdP").empty();
            $.each(data, function (index, row) {

                $("#IdP").append("<option id='IdPi' data-value=" + row.IdP + " value='" + row.IdP + "'>" + row.NameP + "</option>")

            });

        });
    });
})