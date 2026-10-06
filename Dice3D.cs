namespace Kaskrout;

/// <summary>Self-contained CSS 3D die rendered by the Android WebView.</summary>
internal static class Dice3D
{
    public const string Page = """
        <!doctype html>
        <html lang="fr">
        <head>
          <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
          <style>
            * { box-sizing: border-box; }
            html, body { margin: 0; width: 100%; height: 100%; overflow: hidden; background: transparent; }
            body { display: grid; place-items: center; font-family: sans-serif; }
            .scene { position: relative; width: 250px; height: 250px; perspective: 850px; }
            .shadow { position: absolute; left: 50%; top: 76%; width: 150px; height: 32px; border-radius: 50%;
              transform: translateX(-50%) rotateX(62deg); background: radial-gradient(ellipse, rgba(72,12,15,.38), rgba(72,12,15,0) 72%);
              filter: blur(4px); }
            .tilt { position: absolute; inset: 0; display: grid; place-items: center;
              transform-style: preserve-3d; transform: rotateX(-25deg) rotateY(-30deg); }
            .turner { position: relative; width: 168px; height: 168px; transform-style: preserve-3d;
              transform: rotateX(0deg) rotateY(0deg) rotateZ(0deg); }
            .face { position: absolute; inset: 0; border: 1px solid rgba(111,12,17,.7); border-radius: 20px;
              backface-visibility: hidden; overflow: hidden; transform-style: preserve-3d;
              box-shadow: inset 0 2px 3px rgba(255,255,255,.58), inset 0 -7px 12px rgba(90,0,8,.20), 0 0 1px 1px rgba(255,255,255,.18); }
            .front { transform: translateZ(84px); background: linear-gradient(145deg,#f63736 0%,#e51e25 55%,#c90e1a 100%); }
            .back { transform: rotateY(180deg) translateZ(84px); background: linear-gradient(145deg,#c81820,#a90c17); }
            .right { transform: rotateY(90deg) translateZ(84px); background: linear-gradient(145deg,#d91c24,#a90c17); }
            .left { transform: rotateY(-90deg) translateZ(84px); background: linear-gradient(145deg,#ef292e,#bd111b); }
            .top { transform: rotateX(90deg) translateZ(84px); background: linear-gradient(145deg,#ff5149,#e51b23 75%); }
            .bottom { transform: rotateX(-90deg) translateZ(84px); background: linear-gradient(145deg,#bd121d,#960a13); }
            .pip { position: absolute; width: 21px; height: 21px; border-radius: 50%;
              background: radial-gradient(circle at 32% 28%, #fff 0 28%, #f7f7f7 54%, #d6d6d6 100%);
              box-shadow: 1px 2px 3px rgba(70,0,0,.44), inset -1px -2px 2px rgba(150,150,150,.30), inset 1px 1px 1px #fff; }
            .tl { left: 19%; top: 19%; } .tc { left: 50%; top: 19%; transform: translateX(-50%); }
            .tr { right: 19%; top: 19%; } .ml { left: 19%; top: 50%; transform: translateY(-50%); }
            .mc { left: 50%; top: 50%; transform: translate(-50%,-50%); } .mr { right: 19%; top: 50%; transform: translateY(-50%); }
            .bl { left: 19%; bottom: 19%; } .bc { left: 50%; bottom: 19%; transform: translateX(-50%); }
            .br { right: 19%; bottom: 19%; }
            @media (max-height: 240px) { .scene { transform: scale(.8); } }
          </style>
        </head>
        <body>
          <div class="scene">
            <div class="shadow"></div>
            <div class="tilt"><div class="turner" id="turner">
              <div class="face front"><i class="pip tl"></i><i class="pip tr"></i><i class="pip bl"></i><i class="pip br"></i></div>
              <div class="face back"><i class="pip tl"></i><i class="pip mc"></i><i class="pip br"></i></div>
              <div class="face right"><i class="pip mc"></i></div>
              <div class="face left"><i class="pip tl"></i><i class="pip tr"></i><i class="pip ml"></i><i class="pip mr"></i><i class="pip bl"></i><i class="pip br"></i></div>
              <div class="face top"><i class="pip tl"></i><i class="pip tr"></i><i class="pip mc"></i><i class="pip bl"></i><i class="pip br"></i></div>
              <div class="face bottom"><i class="pip tl"></i><i class="pip br"></i></div>
            </div></div>
          </div>
          <script>
            const turner = document.getElementById('turner');
            let angleX = 0, angleY = 0, angleZ = 0, busy = false;
            const target = { 1:[0,-90,0], 2:[90,0,0], 3:[0,180,0], 4:[0,0,0], 5:[-90,0,0], 6:[0,90,0] };
            function rollDice(value) {
              if (busy) return;
              busy = true;
              const [tx,ty,tz] = target[value];
              const sx = angleX, sy = angleY, sz = angleZ;
              const ex = tx + 360 * Math.round((sx + 900 - tx) / 360);
              const ey = ty + 360 * Math.round((sy + 1260 - ty) / 360);
              const ez = tz + 360 * Math.round((sz + 720 - tz) / 360);
              const frames = [];
              for (let i=0; i<=28; i++) {
                const t = i/28, ease = 1 - Math.pow(1-t, 2.1);
                const x = sx + (ex-sx)*ease + Math.sin(t*Math.PI)*110;
                const y = sy + (ey-sy)*ease - Math.sin(t*Math.PI)*75;
                const z = sz + (ez-sz)*ease + Math.sin(t*Math.PI)*45;
                frames.push({transform:`rotateX(${x}deg) rotateY(${y}deg) rotateZ(${z}deg)`});
              }
              const animation = turner.animate(frames, {duration:1450, easing:'cubic-bezier(.18,.78,.25,1)', fill:'forwards'});
              animation.onfinish = () => {
                angleX=tx; angleY=ty; angleZ=tz;
                turner.style.transform=`rotateX(${tx}deg) rotateY(${ty}deg) rotateZ(${tz}deg)`;
                animation.cancel(); busy=false;
              };
            }
          </script>
        </body>
        </html>
        """;
}
