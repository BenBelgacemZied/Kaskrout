using Microsoft.Maui.Controls.Shapes;

namespace Kaskrout;

public sealed class SnakeGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.Snake;

    public override void Launch()
    {
        StartPage("Snake", "Guide le serpent et mange les pommes.");
        var labels = System.Text.Json.JsonSerializer.Serialize(new
        {
            score = F("SCORE", "SCORE", "SCORE"),
            best = F("RECORD", "BEST", "RECORD"),
            hint = F("Glisse ou utilise les flèches", "Swipe or use the arrows", "Veeg of gebruik de pijlen"),
            gameOver = F("PARTIE TERMINÉE", "GAME OVER", "SPEL AFGELOPEN"),
            finalScore = F("Score", "Score", "Score"),
            restart = F("REJOUER", "PLAY AGAIN", "OPNIEUW SPELEN")
        });
        var html = """
<!doctype html><html><head><meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no">
<style>
*{box-sizing:border-box}html,body{margin:0;width:100%;height:100%;overflow:hidden;background:#102b21;color:#fff;font-family:system-ui,sans-serif}
#app{height:100%;display:flex;flex-direction:column;padding:10px;gap:8px;background:linear-gradient(160deg,#164833,#0d2d24)}
#top{display:flex;gap:8px}.stat{flex:1;text-align:center;border:2px solid #8bc77c;border-radius:10px;padding:5px;background:#17432f;box-shadow:0 3px #082219}
small{display:block;font-size:10px;font-weight:800;letter-spacing:1px;color:#bde8ad}.num{font-size:20px;font-weight:900;color:#ffe66d}
#board{position:relative;flex:1;min-height:230px;border:4px solid #a4d887;border-radius:12px;overflow:hidden;box-shadow:0 5px #082219}
canvas{width:100%;height:100%;display:block;touch-action:none}
#overlay{display:none;position:absolute;inset:0;background:#07170fe8;align-items:center;justify-content:center;flex-direction:column;gap:10px;text-align:center}
#overlay .emoji{font-size:54px}#overlay .message{font-size:21px;font-weight:900;color:#ffe66d}
#again{border:2px solid #ffe66d;border-radius:10px;background:#35a65b;color:white;font-size:14px;font-weight:900;padding:10px 18px}
#hint{text-align:center;color:#d4edcf;font-size:12px;font-weight:700}
#pad{display:grid;grid-template-columns:repeat(3,1fr);grid-template-rows:42px 42px;gap:5px;width:180px;align-self:center}
.arrow{border:1px solid #a4d887;border-radius:9px;background:#246b43;color:#fff;font-size:20px;font-weight:900;touch-action:manipulation}
#up{grid-column:2;grid-row:1}#left{grid-column:1;grid-row:2}#down{grid-column:2;grid-row:2}#right{grid-column:3;grid-row:2}
</style></head><body>
<div id="app"><div id="top">
<div class="stat"><small id="scoreLabel"></small><span id="score" class="num">0</span></div>
<div class="stat"><small id="bestLabel"></small><span id="best" class="num">0</span></div>
</div><div id="board"><canvas id="game"></canvas><div id="overlay"><div class="emoji">🐍💫</div><div class="message" id="endMessage"></div><button id="again"></button></div></div>
<div id="hint"></div><div id="pad">
<button class="arrow" id="up">↑</button><button class="arrow" id="left">←</button><button class="arrow" id="down">↓</button><button class="arrow" id="right">→</button>
</div></div>
<script>
const L=__LABELS__;
document.getElementById('scoreLabel').textContent=L.score;
document.getElementById('bestLabel').textContent=L.best;
document.getElementById('hint').textContent=L.hint;
document.getElementById('again').textContent=L.restart;
const canvas=document.getElementById('game'),ctx=canvas.getContext('2d'),board=document.getElementById('board');
const cols=20,rows=24,cell=16;
let snake,dir,nextDir,food,score,best=Number(localStorage.getItem('snake-best')||0),alive,timer,lastTime=0,acc=0,swipeStart=null;
document.getElementById('best').textContent=best;
function resize(){const r=board.getBoundingClientRect(),d=Math.min(devicePixelRatio||1,2);canvas.width=r.width*d;canvas.height=r.height*d;ctx.setTransform(d,0,0,d,0,0);draw()}
new ResizeObserver(resize).observe(board);
function reset(){snake=[{x:9,y:12},{x:8,y:12},{x:7,y:12}];dir={x:1,y:0};nextDir={x:1,y:0};score=0;alive=true;acc=0;lastTime=0;document.getElementById('score').textContent=0;document.getElementById('overlay').style.display='none';placeFood();requestAnimationFrame(loop)}
function placeFood(){do{food={x:Math.floor(Math.random()*cols),y:Math.floor(Math.random()*rows)}}while(snake.some(p=>p.x===food.x&&p.y===food.y))}
function turn(x,y){if(!alive||x===-dir.x&&y===-dir.y)return;nextDir={x,y}}
document.getElementById('up').onclick=()=>turn(0,-1);document.getElementById('down').onclick=()=>turn(0,1);document.getElementById('left').onclick=()=>turn(-1,0);document.getElementById('right').onclick=()=>turn(1,0);
board.addEventListener('pointerdown',e=>{swipeStart={x:e.clientX,y:e.clientY}});
board.addEventListener('pointerup',e=>{if(!swipeStart)return;let dx=e.clientX-swipeStart.x,dy=e.clientY-swipeStart.y;swipeStart=null;if(Math.max(Math.abs(dx),Math.abs(dy))<18)return;if(Math.abs(dx)>Math.abs(dy))turn(dx>0?1:-1,0);else turn(0,dy>0?1:-1)});
document.getElementById('again').onclick=reset;
function finish(){alive=false;let e=document.getElementById('overlay');document.getElementById('endMessage').textContent=L.gameOver+' — '+L.finalScore+': '+score;e.style.display='flex';if(score>best){best=score;localStorage.setItem('snake-best',best);document.getElementById('best').textContent=best}}
function step(){dir=nextDir;let head={x:snake[0].x+dir.x,y:snake[0].y+dir.y};if(head.x<0||head.x>=cols||head.y<0||head.y>=rows||snake.some((p,i)=>i>0&&p.x===head.x&&p.y===head.y)){finish();return}snake.unshift(head);if(head.x===food.x&&head.y===food.y){score+=10;document.getElementById('score').textContent=score;placeFood();location.href='kaskrout://food?at='+Date.now()}else snake.pop();draw()}
function draw(){const w=canvas.clientWidth,h=canvas.clientHeight;if(!w||!h)return;let size=Math.min(w/cols,h/rows),ox=(w-size*cols)/2,oy=(h-size*rows)/2;ctx.clearRect(0,0,w,h);ctx.fillStyle='#0a261d';ctx.fillRect(0,0,w,h);
ctx.strokeStyle='#174333';ctx.lineWidth=1;for(let x=0;x<=cols;x++){ctx.beginPath();ctx.moveTo(ox+x*size,oy);ctx.lineTo(ox+x*size,oy+rows*size);ctx.stroke()}for(let y=0;y<=rows;y++){ctx.beginPath();ctx.moveTo(ox,oy+y*size);ctx.lineTo(ox+cols*size,oy+y*size);ctx.stroke()}
ctx.fillStyle='#ff5b59';ctx.beginPath();ctx.arc(ox+(food.x+.5)*size,oy+(food.y+.5)*size,size*.38,0,Math.PI*2);ctx.fill();ctx.fillStyle='#b8ef75';ctx.fillRect(ox+(food.x+.42)*size,oy+(food.y+.10)*size,size*.18,size*.22);
snake.forEach((p,i)=>{ctx.fillStyle=i===0?'#d7ff79':'#67d17a';ctx.fillRect(ox+p.x*size+1,oy+p.y*size+1,size-2,size-2);if(i===0){ctx.fillStyle='#17351f';let ex=dir.x===1?.67:dir.x===-1?.25:.33,ey=dir.y===1?.67:dir.y===-1?.25:.3;ctx.fillRect(ox+(p.x+ex)*size,oy+(p.y+ey)*size,size*.13,size*.13);ctx.fillRect(ox+(p.x+ex+(dir.y!==0?.35:0))*size,oy+(p.y+ey+(dir.x!==0?.35:0))*size,size*.13,size*.13)}})}
function loop(t){if(!alive)return;if(!lastTime)lastTime=t;acc+=t-lastTime;lastTime=t;let interval=Math.max(65,160-score*.8);if(acc>=interval){acc=0;step()}if(alive)requestAnimationFrame(loop)}
reset();
</script></body></html>
""";
        html = html.Replace("__LABELS__", labels);
        var game = new WebView
        {
            Source = new HtmlWebViewSource { Html = html },
            HeightRequest = 540,
            BackgroundColor = Color.FromArgb("#102B21"),
            HorizontalOptions = LayoutOptions.Fill
        };
        game.Navigating += (_, e) =>
        {
            if (e.Url?.StartsWith("kaskrout://food", StringComparison.OrdinalIgnoreCase) == true)
            {
                e.Cancel = true;
                AddPoints(1);
            }
        };
        body.Children.Add(new Border
        {
            BackgroundColor = Color.FromArgb("#102B21"),
            Stroke = Color.FromArgb("#6DAE66"), StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            Padding = 5, Content = game
        });
    }
}
