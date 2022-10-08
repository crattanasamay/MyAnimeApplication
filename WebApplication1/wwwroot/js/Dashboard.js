$("#mainView").click(() => {
    $("#ChildDashboardView").remove();
    $.post("/AnimeLikedPartial/", function (data) {
        $("#MainDashboardView").append(data);
    });
  
});

// send request to anime partial season using post method
function LoadAnimeSeason(event) {
    var seasonString = (event.target.id).toString();
    if (seasonString.search("_") != -1) {
        $.post("/AnimeSeason/", { id: seasonString }, function (data) {
            $("#ChildDashboardView").remove();
            $("#MainDashboardView").append(data);
        });
    }
    else {
        //Do something
    }
};


$("#anime_history").click((event) => {
    $("#ChildDashboardView").remove();
    $.post("/AnimeHistory/", function (data) {
        $("#MainDashboardView").append(data);
    });
})


// If a user clicks the season menu
$("ul#submenu1 > li > a > span").click((event) => {

    LoadAnimeSeason(event); // send event to load partial view
});


$(document).ready(() => {
    $("#ChildDashboardView").remove();
    $.post("/AnimeLikedPartial/", function (data) {
        $("#MainDashboardView").append(data);
    });
  
   
});
//wait for the DOM to finish loading





