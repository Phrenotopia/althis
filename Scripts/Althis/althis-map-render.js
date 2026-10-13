//var width = 16;
//var height = 16;

function DrawCanvas(tiles, terrain, startpos, scale, width, height) {
    //alert("in: " + startpos + "");
    $("#mapdraw").html("Drawing canvas - start: " + startpos + "<br/>" +
                       //"terrain: " + terrain.src + "<br/>" +
                       "tiles: " + tiles.length + "<br/>" +
                       "scale: " + scale + " <br/>" +
                       "height: " + height + " <br/>" +
                       "width: " + width + " <br/>");

    var canvas = document.getElementById('myCanvas');
    var context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);
    var i = startpos;
    for (var y = 0; y < height * scale; y += scale) {
        for (var x = 0; x < width * scale; x += scale) {
            var tile = tiles[i].Elevation;
            var xyfactor = Math.floor(tile / width);
            var yoffset = xyfactor * 100;
            var xoffset = (tile - (width * xyfactor)) * 100;
            context.drawImage(terrain, xoffset, yoffset, 100, 100, x, y, scale, scale);
            i++;
        }
    }
}

