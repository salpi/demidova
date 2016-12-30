var pole, width, height;
var ncheck = 0,
    icheck;
var ctx, pic, cs;

function PaintPole(pic, ctx, mas) {

    var n = mas.length;

    var x, y, xp, yp, dx, dy,dcx,dcy;
    for (var i = 0; i < n; i++) {
        x = i % width;
        y = parseInt(i / width);

        xp = mas[i].num % width;
        yp = parseInt(mas[i].num / width);

        dx = pic.width / width;
        dy = pic.height / height;
		
		dcx = cs.width / width;
        dcy = cs.height /(2* height);
		
        if (mas[i].open)
            ctx.drawImage(pic, xp * dx, yp * dy, dx, dy, x * dcx, y * dcy, dcx, dcy);
        else {
            ctx.fillStyle = "rgb(0, 0,0)";
            ctx.fillRect(x * dcx, y * dcy, dcx, dcy);
            ctx.lineWidth = 2;
            ctx.strokeStyle = "rgb(255, 0, 0)";
            ctx.strokeRect(x * dcx, y * dcy, dcx, dcy);
        }

    }
}
var fcont = function(e) {
    if (e.pageX || e.pageY) {
        //Событие click у текущей фигуры
        var x, y;
        x = e.pageX - this.getBoundingClientRect().left;
        y = e.pageY - this.getBoundingClientRect().top;

        var xn, yn;
        xn = parseInt(x / (cs.width / width));
        yn = parseInt(y / (cs.height / (height * 2)));
        document.getElementById("xy").innerHTML = xn + ";" + yn;

        //по завершению (открытия всех пазлов) вывести картинку ориганал в верхней половине канваса
        var indEl = yn * width + xn;
        if (pole.mas[indEl].open == true) return;
        pole.mas[indEl].open = true;
        ncheck++;
        PaintPole(pic, ctx, pole.mas);
        if (ncheck <= 1) {
            icheck = indEl;
            return;
        }
 
        if (pole.mas[icheck].num == pole.mas[indEl].num) {
            ncheck = 0;
			openPic();
            return;
        }
        ncheck = 0;
       	setTimeout(closePazzle,2000,indEl,icheck);

    }
	
}
function closePazzle(indEl,icheck){
	 pole.mas[indEl].open = pole.mas[icheck].open = false;
     PaintPole(pic, ctx, pole.mas);	
}
function openPic(){
	var nopen=0;
	for(var i=0;i<pole.mas.length;i++){
		if(pole.mas[i].open)nopen++;
	}
	if(nopen==pole.mas.length)
		ctx.drawImage(pic,0,0,cs.width,cs.height/2);
}

function Start() {
    pole = new Pole();
    width = document.getElementById("elWidth").value;
    height = document.getElementById("elHeight").value;
    pole.Construct(width, height);

    var scheme = "scheme.jpg";
	scheme = "https://yandex.ru/images/today?size=1280x1024";
    //в той же директории что и скрипт
    if (document.getElementById("scheme").files[0] != undefined)
        scheme = document.getElementById("scheme").files[0].name;
    cs = document.getElementById("cs");
    ctx = cs.getContext('2d');
    pic = new Image();
    pic.onload = function() {
		var clheight=document.body.clientHeight*0.4;
        cs.width = (clheight/pic.height)*pic.width;
        cs.height = (clheight/pic.height)*pic.height * 2;
        PaintPole(pic, ctx, pole.mas);
    };
    pic.src = scheme;
    cs.removeEventListener("click", fcont);
    cs.addEventListener("click", fcont);


}

function Cell(num, open) {
    this.num = num;
    this.open = open;
}

function Pole() {
    this.mas = [];
    this.width;
    this.height;
    this.Construct = function(width, height) {
        //if ((width * height) % 2 > 0) return false;
        this.height = height;
        this.width = width;
        var n = height * width;
        //mas=new Array(n);
        for (var i = 0; i < n; i++) {
            //Пример: Случайное целое между min и max
            this.mas[i] = new Cell(i, false);
            this.mas[i + n] = new Cell(i, false);
            // mas[i+n] = new Cell(i,false);
        }
        for (var i = 0; i < n; i++) {

        }
        var rnd, t;
        for (var i = 0; i < n * 2; i++) {
            // Math.random() Cлучайное число от 0(включительно) до 1
            //Случайное целое между min и max
            rnd = Math.floor(Math.random() * (2 * n - 0 + 0)) + 0;
            t = this.mas[i].num;
            this.mas[i].num = this.mas[rnd].num;
            this.mas[rnd].num = t;
        }
    }
}