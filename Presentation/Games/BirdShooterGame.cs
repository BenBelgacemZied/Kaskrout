namespace Kaskrout;

public sealed class BirdShooterGame(IGameUiHost host) : GameModuleBase(host)
{
    public override string Id => GameIds.BirdShooter;

    public override void Launch()
    {
        StartPage("Chasse aux oiseaux – rétro", "Vise les oiseaux et appuie pour tirer.");
        var labels = System.Text.Json.JsonSerializer.Serialize(new
        {
            score = F("SCORE", "SCORE", "SCORE"),
            level = F("NIVEAU", "LEVEL", "NIVEAU"),
            birds = F("OISEAUX", "BIRDS", "VOGELS"),
            hint = F("Glisse pour viser • Touche pour tirer", "Drag to aim • Tap to shoot", "Sleep om te richten • Tik om te schieten"),
            restart = F("NOUVELLE PARTIE", "NEW GAME", "NIEUW SPEL"),
            success = F("Bravo ! Les 10 oiseaux sont touchés !", "Great! All 10 birds hit!", "Goed gedaan! Alle 10 vogels geraakt!"),
            failure = F("Oups ! Les oiseaux se sont échappés…", "Oops! The birds got away…", "Oeps! De vogels zijn ontsnapt…"),
            tease = F("Le chien rigole : tu retentes ?", "The dog is laughing. Try again?", "De hond lacht. Nog eens proberen?"),
            next = F("NIVEAU SUIVANT", "NEXT LEVEL", "VOLGEND NIVEAU"),
            retry = F("RÉESSAYER", "TRY AGAIN", "OPNIEUW PROBEREN")
        });
        var html = """
<!doctype html>
<html lang="fr"><head><meta name="viewport" content="width=device-width,initial-scale=1,maximum-scale=1,user-scalable=no">
<style>
*{box-sizing:border-box}html,body{margin:0;width:100%;height:100%;overflow:hidden;background:#111938;font-family:system-ui,sans-serif;color:#fff}
#shell{height:100%;display:flex;flex-direction:column;padding:12px;gap:9px;background:linear-gradient(180deg,#17214b,#29215d 62%,#111938)}
#hud{display:flex;justify-content:space-between;gap:7px}.stat{flex:1;text-align:center;padding:8px 4px;border:2px solid #6576df;border-radius:10px;background:#212951;box-shadow:0 3px #10152f}
small{display:block;font-size:10px;font-weight:800;letter-spacing:1px;color:#b9c7ff}.value{font-size:21px;font-weight:900;color:#ffe66d}
#screen{position:relative;flex:1;min-height:260px;border:4px solid #7785ed;border-radius:12px;overflow:hidden;background:#70c8ee;box-shadow:0 6px #10152f}
canvas{width:100%;height:100%;display:block;touch-action:none}
#hint{text-align:center;color:#cbd3ff;font-size:12px;font-weight:700}
#restart{align-self:center;border:2px solid #ffe66d;border-radius:10px;background:#d94d68;color:white;font-weight:900;font-size:14px;padding:10px 24px;box-shadow:0 4px #812b50}
#end{display:none;position:absolute;inset:0;background:#111938e8;align-items:center;justify-content:center;flex-direction:column;gap:10px;padding:18px;text-align:center;font-weight:900;font-size:20px;color:#ffe66d}
#dog{font-size:64px;filter:drop-shadow(0 4px 0 #080d22)}#endText{line-height:1.35}
#endButton{border:2px solid #ffe66d;border-radius:10px;background:#d94d68;color:white;font-weight:900;font-size:13px;padding:10px 17px;box-shadow:0 4px #812b50}
</style></head><body>
<div id="shell"><div id="hud">
<div class="stat"><small id="scoreLabel"></small><span class="value" id="score">0</span></div>
<div class="stat"><small id="levelLabel"></small><span class="value" id="level">1</span></div>
<div class="stat"><small id="birdsLabel"></small><span class="value" id="birdCount">0/10</span></div>
</div>
<div id="screen"><canvas id="game"></canvas><div id="end"><div id="dog"></div><div id="endText"></div><button id="endButton"></button></div></div>
<div id="hint"></div><button id="restart"></button></div>
<script>
const L=__LABELS__;
document.getElementById('scoreLabel').textContent=L.score;
document.getElementById('levelLabel').textContent=L.level;
document.getElementById('birdsLabel').textContent=L.birds;
document.getElementById('hint').textContent=L.hint;
document.getElementById('restart').textContent='↻  '+L.restart;
const canvas=document.getElementById('game'),ctx=canvas.getContext('2d'),screen=document.getElementById('screen');
let w=0,h=0,score=0,level=1,hitCount=0,spawned=0,birds=[],shots=[],sparks=[],aim={x:0,y:0},last=0,spawn=0,ended=false,roundWon=false,audio;
const BIRDS_PER_LEVEL=10;
function resize(){let r=screen.getBoundingClientRect(),d=Math.min(devicePixelRatio||1,2);w=r.width;h=r.height;canvas.width=w*d;canvas.height=h*d;ctx.setTransform(d,0,0,d,0,0);aim.x=w/2;aim.y=h*.48}
new ResizeObserver(resize).observe(screen);resize();
function tone(freq,dur=.07,type='square'){try{audio=audio||new(window.AudioContext||window.webkitAudioContext)();let o=audio.createOscillator(),g=audio.createGain();o.type=type;o.frequency.value=freq;g.gain.setValueAtTime(.08,audio.currentTime);g.gain.exponentialRampToValueAtTime(.001,audio.currentTime+dur);o.connect(g);g.connect(audio.destination);o.start();o.stop(audio.currentTime+dur)}catch(e){}}
function addBird(){if(spawned>=BIRDS_PER_LEVEL)return;let fromLeft=Math.random()<.5,golden=Math.random()<.18,speed=Math.min(8,1.6+(level-1)*.42+Math.random());spawned++;birds.push({x:fromLeft?-35:w+35,y:35+Math.random()*(h*.58),vx:(fromLeft?1:-1)*speed,phase:Math.random()*6.28,size:golden?20:17,golden,alive:true});document.getElementById('birdCount').textContent=hitCount+'/'+BIRDS_PER_LEVEL}
function finishRound(){if(ended)return;ended=true;roundWon=hitCount===BIRDS_PER_LEVEL;let dog=document.getElementById('dog'),text=document.getElementById('endText'),button=document.getElementById('endButton');dog.textContent=roundWon?'🐶🎉':'🐶😂';text.innerHTML=(roundWon?L.success:L.failure)+'<br><span style="font-size:14px;color:#fff">'+(roundWon?'':L.tease)+'</span><br><span style="font-size:13px;color:#cbd3ff">'+hitCount+'/'+BIRDS_PER_LEVEL+'</span>';button.textContent=roundWon?L.next:L.retry;document.getElementById('end').style.display='flex';tone(roundWon?880:220,.25,roundWon?'triangle':'square')}
function resetRound(next){if(next)level++;hitCount=0;spawned=0;birds=[];shots=[];sparks=[];spawn=0;ended=false;roundWon=false;document.getElementById('birdCount').textContent='0/'+BIRDS_PER_LEVEL;document.getElementById('level').textContent=level;document.getElementById('end').style.display='none';last=0}
function point(e){let r=canvas.getBoundingClientRect();aim.x=e.clientX-r.left;aim.y=e.clientY-r.top}
canvas.addEventListener('pointermove',point);
canvas.addEventListener('pointerdown',e=>{e.preventDefault();point(e);if(ended)return;tone(180,.08,'sawtooth');shots.push({x:aim.x,y:aim.y,life:12});let hit=null;for(let b of birds){let dx=aim.x-b.x,dy=aim.y-b.y;if(b.alive&&Math.hypot(dx,dy)<b.size*1.6){hit=b;break}}if(hit){hit.alive=false;hitCount++;score+=hit.golden?30:10;document.getElementById('score').textContent=score;document.getElementById('birdCount').textContent=hitCount+'/'+BIRDS_PER_LEVEL;tone(hit.golden?1040:760,.16,'triangle');for(let i=0;i<14;i++)sparks.push({x:hit.x,y:hit.y,vx:(Math.random()-.5)*5,vy:(Math.random()-.7)*5,life:30,gold:hit.golden});location.href='kaskrout://hit?at='+Date.now()}});
document.getElementById('restart').onclick=()=>{score=0;level=1;document.getElementById('score').textContent='0';resetRound(false)};
document.getElementById('endButton').onclick=()=>resetRound(roundWon);
function bird(b,t){ctx.save();ctx.translate(b.x,b.y);if(b.vx<0)ctx.scale(-1,1);let s=b.size/10;ctx.scale(s,s);let wing=Math.sin(t*.012+b.phase)>0?1:0;ctx.fillStyle=b.golden?'#ffe66d':'#242450';ctx.fillRect(-8,-2,13,8);ctx.fillRect(4,0,7,4);ctx.fillRect(10,-2,3,3);ctx.fillStyle='#ff8b4a';ctx.fillRect(13,0,4,2);ctx.fillStyle=b.golden?'#fff1a6':'#6c75db';ctx.fillRect(-5,wing?-8:4,8,7);ctx.fillStyle='#fff';ctx.fillRect(7,-2,2,2);ctx.fillStyle='#292044';ctx.fillRect(8,-2,1,1);ctx.restore()}
function scene(t){ctx.clearRect(0,0,w,h);let sky=ctx.createLinearGradient(0,0,0,h);sky.addColorStop(0,'#65c7ef');sky.addColorStop(.68,'#b2ebef');sky.addColorStop(.69,'#5e9a58');sky.addColorStop(1,'#315c43');ctx.fillStyle=sky;ctx.fillRect(0,0,w,h);
ctx.fillStyle='#ffe66d';ctx.fillRect(w-55,22,28,28);ctx.fillStyle='#fff';for(let i=0;i<3;i++){let x=(i*143+t*.008)%(w+100)-50,y=45+i*29;ctx.fillRect(x,y,38,8);ctx.fillRect(x+8,y-7,24,8)}
ctx.fillStyle='#506c9b';ctx.beginPath();ctx.moveTo(0,h*.65);for(let x=0;x<=w;x+=32)ctx.lineTo(x,h*.48+Math.sin(x*.025)*13);ctx.lineTo(w,h);ctx.lineTo(0,h);ctx.fill();ctx.fillStyle='#314f42';for(let x=8;x<w;x+=25){ctx.fillRect(x,h*.71,5,35);ctx.fillRect(x-5,h*.73,15,4);ctx.fillRect(x-3,h*.69,11,4)}
}
function frame(t){let dt=Math.min(32,t-last||16);last=t;if(!ended){spawn+=dt;if(spawn>Math.max(350,1150-level*55)){addBird();spawn=0}birds.forEach(b=>{if(!b.alive)return;b.x+=b.vx*dt/16;b.phase+=dt*.01;if((b.vx>0&&b.x>w+45)||(b.vx<0&&b.x<-45)){b.alive=false}});if(spawned===BIRDS_PER_LEVEL&&birds.every(b=>!b.alive))finishRound()}
scene(t);birds.forEach(b=>{if(b.alive)bird(b,t)});shots=shots.filter(s=>s.life-->0);shots.forEach(s=>{ctx.strokeStyle='rgba(255,90,105,'+s.life/12+')';ctx.lineWidth=3;ctx.beginPath();ctx.moveTo(w/2,h-8);ctx.lineTo(s.x,s.y);ctx.stroke()});sparks=sparks.filter(p=>p.life-->0);sparks.forEach(p=>{p.x+=p.vx;p.y+=p.vy;p.vy+=.08;ctx.fillStyle=p.gold?'#ffe66d':'#fff';ctx.fillRect(p.x,p.y,4,4)});
ctx.strokeStyle='#fff';ctx.lineWidth=2;ctx.beginPath();ctx.arc(aim.x,aim.y,12,0,Math.PI*2);ctx.moveTo(aim.x-19,aim.y);ctx.lineTo(aim.x-6,aim.y);ctx.moveTo(aim.x+6,aim.y);ctx.lineTo(aim.x+19,aim.y);ctx.moveTo(aim.x,aim.y-19);ctx.lineTo(aim.x,aim.y-6);ctx.moveTo(aim.x,aim.y+6);ctx.lineTo(aim.x,aim.y+19);ctx.stroke();requestAnimationFrame(frame)}
requestAnimationFrame(frame);
</script></body></html>
""";
        html = html.Replace("__LABELS__", labels);
        var game = new WebView
        {
            Source = new HtmlWebViewSource { Html = html },
            HeightRequest = 510,
            BackgroundColor = Color.FromArgb("#171D46"),
            HorizontalOptions = LayoutOptions.Fill
        };
        game.Navigating += (_, e) =>
        {
            if (e.Url?.StartsWith("kaskrout://hit", StringComparison.OrdinalIgnoreCase) == true)
            {
                e.Cancel = true;
                AddPoints(1);
            }
        };
        body.Children.Add(new Border
        {
            BackgroundColor = Color.FromArgb("#171D46"),
            Stroke = Color.FromArgb("#5C67C8"), StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            Padding = 5, Content = game
        });
    }
}
