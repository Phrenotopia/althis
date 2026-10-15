


function Mouse() {
    this.posX = 0;
    this.posY = 0;
    this.mouseDown = false;
}

Mouse.prototype.getMousePos = function (e) {
    posX = e.pageX-this.offsetLeft;
    posY = e.pageY-this.offsetTop;
}