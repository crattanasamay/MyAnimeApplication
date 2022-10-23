$("#mainView").click(() => {
    DeleteMainView();
    AddMainDashboardView();  
    $.post("/UserAnimeChart/", (data) => {
        $("#UserChartDashboardView").append(data);
    });
    $.post("/AnimeLikedPartial/", function (data) {
        $("#MainDashboardView").append(data);
    });
    $.post("/AverageScoreTopAnimeChart/", (data) => {
        $("#AverageAnimeTopChartView").append(data);
    });

    
  
});

// send request to anime partial season using post method
function LoadAnimeSeason(event) {
    DeleteMainView();
    var seasonString = (event.target.id).toString();
    if (seasonString.search("_") != -1) {
        $.post("/AnimeSeason/", { id: seasonString }, function (data) {
            $("#DashboardView").append(data);
        });
    }
    else {
        //Do something
    }
};


$("#anime_history").click((event) => {
    DeleteMainView();
    $.post("/AnimeHistory/", function (data) {
        $("#DashboardView").append(data);
    });
})


// If a user clicks the season menu
$("ul#submenu1 > li > a > span").click((event) => {

    LoadAnimeSeason(event); // send event to load partial view
});




$(document).ready(() => {
    DeleteMainView();
    AddMainDashboardView();
    $.post("/AnimeLikedPartial/", function (data) {
        $("#MainDashboardView").append(data);
    });

    $.post("/UserAnimeChart/", (data) => {
        $("#UserChartDashboardView").append(data);
    });
    $.post("/AverageScoreTopAnimeChart/", (data) => {
        $("#AverageAnimeTopChartView").append(data);
    });

    
});
function AddMainDashboardView() {
    $('<div class="container-fluid" id="MainDashboardView"></div>').appendTo('#DashboardView');
    $('<div class="container-fluid" id="UserChartDashboardView"></div>').appendTo('#DashboardView');
    $('<div class="container-fluid" id="AverageAnimeTopChartView"></div>').appendTo('#DashboardView');
}
function DeleteMainView() {
    $("#DashboardView").html("");
}





