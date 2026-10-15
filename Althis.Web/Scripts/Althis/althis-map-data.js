

function LoadMap(mapid, startpos, scale) {
    var tiles = [];
    var url = '/api/map/section/' + mapid + '/' + startpos + '/' + width + '/' + height;
    $("#debug").append(url);

    $.getJSON(url)
        .done(function (data) {
            $.each(data, function (key, val) {
                tiles.push(val.Elevation);
            });

            return tiles;

        }).fail(function () { alert("fail!") });
}

function LoadMap(mapid) {
    var tiles = [];
    var url = '/api/map/' + mapid;

    $.getJSON(url)
        .done(function (data) {
            $.each(data, function (key, val) {
                tiles.push(val.Elevation);
            });

        }).fail(function () { alert("fail!") });
}
