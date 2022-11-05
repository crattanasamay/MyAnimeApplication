$("#mainView").click(() => {
    DeleteMainView();
    AddMainDashboardView();  
    $.post("/UserAnimeChart/", (data) => {
        $("#ChartRowOne").append(data);
    });
    $.post("/AnimeLikedPartial/", function (data) {
        $("#MainDashboardView").append(data);
    });
    $.post("/AverageScoreTopAnimeChart/", (data) => {
        $("#ChartRowOne").append(data);
    });

    $.post("/AnimeSeasonGenreChart/", function (data) {
        $("#ChartRowTwo").append(data);
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

$("ul#FUCK").click(() => {
    alert("WTF");
});




$(document).ready(() => {
    $('dropdown-toggle').dropdown()

    DeleteMainView();
    AddMainDashboardView();
    $.post("/AnimeLikedPartial/", function (data) {
        $("#MainDashboardView").append(data);
    });

    $.post("/UserAnimeChart/", (data) => {
        $("#ChartRowOne").append(data);
    });
    $.post("/AverageScoreTopAnimeChart/", (data) => {
        $("#ChartRowOne").append(data);
    });

    $.post("/AnimeSeasonGenreChart/", function (data) {
        $("#ChartRowTwo").append(data);
    });


    
});
function AddMainDashboardView() {
    $('<div class="container-fluid" id="MainDashboardView" style=""></div>').appendTo('#DashboardView');
    $('<div class="container-fluid" style="padding-left:1.5rem;"><div class="row" id="ChartRowOne"></div></div>').appendTo('#DashboardView');
    $('<div class="container-fluid" style="padding-left:1.5rem; padding-top: 1.5rem;"><div class="row" id="ChartRowTwo"></div></div>').appendTo('#DashboardView');

}
function DeleteMainView() {
    $("#DashboardView").html("");
    $("#ChildDashboardView").html("");
}





