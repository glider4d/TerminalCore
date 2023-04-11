function Nav_ScrollIntoView(navId)
{
    alert(navId + '123');return;
    var elem = document.getElementById(navId);
    if (elem === null) return;
    elem.scrollIntoView( { behavior: 'smooth' } );
}

function Nav_ScrollIntoView(className){
    alert("className");return;
    var elem = document.getElementsByClassName(className);
    if(elem === null) return;
    elem.scrollIntoView({behavior: 'smooth'});
}

function ScrollIntoClass(className){
    ///alert(className); ;
    var elem = document.getElementById(className);
    if(elem === null) return;
    elem.scrollIntoView({block: "center", behavior: "smooth"});
}

function test(text){
    alert(text);
}

window.MyWinBox = {
    OpenTestWindow: async() => {
        
        let containerElement = new WinBox({title:'Camera',     class: ["boxtheme","no-full", "no-max", "my-theme"],
        border:'4px double',      width:'1000px',
        height:'1000px', background:'white',html:'<h1><123/h1>'});
        //WinBox("WinBox.js");
        
        await Blazor.rootComponents.add(containerElement.body, 'video', {});
    }
}
var video = document.getElementById('video');

var constraints = {
    audio: false,
    video: {
        width: { ideal: 1280 },
        height: { ideal: 1024 },
        facingMode: "environment"
    }
};

function camera2(){
    video = document.getElementById('video');
    // Get access to the camera!
    if(navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        // Not adding `{ audio: true }` since we only want video now
        navigator.mediaDevices.getUserMedia({ video: true }).then(function(stream) {
            //video.src = window.URL.createObjectURL(stream);
            video.srcObject = stream;
            video.play();
        });
    }
}
function stop(){

}

function play(){

}

function photo(){

}

function camera() {
    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        term.pause();
        var media = navigator.mediaDevices.getUserMedia(constraints);
        media.then(function(mediaStream) {
            term.resume();
            var stream;
            if (!acceptStream) {
                stream = window.URL.createObjectURL(mediaStream);
            } else {
                stream = mediaStream;
            }
            term.echo('<video data-play="true" class="self"></video>', {
                raw: true,
                onClear: function() {
                    if (!acceptStream) {
                        URL.revokeObjectURL(stream);
                    }
                    mediaStream.getTracks().forEach(track => track.stop());
                },
                finalize: function(div) {
                    var video = div.find('video');
                    if (!video.length) {
                        return;
                    }
                    if (acceptStream) {
                        video[0].srcObject = stream;
                    } else {
                        video[0].src = stream;
                    }
                    if (video.data('play')) {
                        video[0].play();
                    }
                }
            });
        });
    }
}

function AboutBox(){
    alert("abcdefgh");
    //let containerElement = new WinBox("WinBox.js");
    //await Blazor.rootComponents.add(containerElement.body, 'counter', {});

    /*
    const AboutBox = new WinBox({
        title: 'About me',

        background:'#00aa00',
        width:'400px',
        height:'400px',
        top:50,
        left:50,
        right:50,
        bottom:50,
        html:'<h1>123213</h1>'
        // mount:'kk'
    });*/
    /*
    const AboutBox = new WinBox({
        title: 'About me',
        background:'#00aa00',
        width:'400px',
        height:'400px',
        top:50,
        right:50,
        bottom:50,
        left:50,
        html:'<h1><Kukluksklan/h1>',
        mount:'aboutContent'
    });*/
}

particlesJS("background", {
    "particles": {
        "number": {
            "value": 80,
            "density": {
                "enable": true,
                "value_area": 800
            }
        },
        "color": {
            "value": color
        },
        "shape": {
            "type": "circle",
            "stroke": {
                "width": 0,
                "color": "#000000"
            },
            "polygon": {
                "nb_sides": 5
            },
            "image": {
                "src": "img/github.svg",
                "width": 100,
                "height": 100
            }
        },
        "opacity": {
            "value": 0.5,
            "random": false,
            "anim": {
                "enable": false,
                "speed": 1,
                "opacity_min": 0.1,
                "sync": false
            }
        },
        "size": {
            "value": 3,
            "random": true,
            "anim": {
                "enable": false,
                "speed": 40,
                "size_min": 0.1,
                "sync": false
            }
        },
        "line_linked": {
            "enable": true,
            "distance": 150,
            "color": color,
            "opacity": 0.4,
            "width": 1
        },
        "move": {
            "enable": true,
            "speed": 6,
            "direction": "none",
            "random": false,
            "straight": false,
            "out_mode": "out",
            "bounce": false,
            "attract": {
                "enable": false,
                "rotateX": 600,
                "rotateY": 1200
            }
        }
    },
    "interactivity": {
        "detect_on": "canvas",
        "events": {
            "onhover": {
                "enable": true,
                "mode": "grab"
            },
            "onclick": {
                "enable": true,
                "mode": "push"
            },
            "resize": true
        },
        "modes": {
            "grab": {
                "distance": 143.85614385614386,
                "line_linked": {
                    "opacity": 1
                }
            },
            "bubble": {
                "distance": 400,
                "size": 40,
                "duration": 2,
                "opacity": 8,
                "speed": 3
            },
            "repulse": {
                "distance": 200,
                "duration": 0.4
            },
            "push": {
                "particles_nb": 4
            },
            "remove": {
                "particles_nb": 2
            }
        }
    },
    "retina_detect": true
});


const cube = document.getElementById("cube");

const clickOnSide = (side) => {
    const activeSide = cube.dataset.side;
    cube.classList.replace(`show-${activeSide}`, `show-${side}`);
    cube.setAttribute("data-side", side);
};

document.querySelectorAll(".btn").forEach((btn) => {
    btn.addEventListener("click", (e) => {
        const sideToTurn = e.target.dataset.side;
        clickOnSide(sideToTurn);
    });
});