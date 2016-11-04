function n() {
    if (window.WebViewJavascriptBridge)
        window.WebViewJavascriptBridge.showMessage("这是新插入的消息");
}

//初始化
var is_first = 1;
var ov = document.getElementsByClassName("other-video");
var last_ov;
if (ov.length > 0) {
    last_ov = ov[0];
    last_ov.setAttribute("data-playing", "1");
    var objs = document.getElementsByClassName("paly-ing");
    if (objs.length < 1) {
        var obj = document.createElement("span");
        obj.setAttribute("class", "paly-ing");
        obj.innerHTML = "正在播放";
        var playBtns = last_ov.getElementsByClassName("play-btn");
        var playBtn = playBtns[0];
        playBtn.parentNode.appendChild(obj);
    }
}
for (var i = 0; i < ov.length; i++) {
    var buts = ov[i].getElementsByClassName("play-btn");
    but = buts[0];
    but.setAttribute("href", "javascript:void(0);");
    but.parentNode.setAttribute("href", "javascript:void(0);");
}

//新链接跳转
var eles1 = document.getElementsByClassName("new-view-link");
for (var i = 0; i < eles1.length; i++) {
    eles1[i].addEventListener("click", function () {
        var obj = document.createElement("div");
        obj.innerHTML = "有得到消息";
        document.body.appendChild(obj);
        var viewId = this.getAttribute("data-id");
        var viewType = this.getAttribute("data-type");
        window.WebViewJavascriptBridge.getMessage("NewView", viewId, viewType);
    }, false);
}

//网页跳转
var eles6 = document.getElementsByClassName("open-url");
for (var i = 0; i < eles6.length; i++) {
    eles6[i].addEventListener("click", function () {
        var obj = document.createElement("div");
        obj.innerHTML = "有得到消息";
        document.body.appendChild(obj);
        var viewUrl = this.getAttribute("data-url");
        window.WebViewJavascriptBridge.getMessage("NewUrl", viewUrl, "");
    }, false);
}

var handler2 = function () {
    var videoPlaying = this.getAttribute("data-playing");
    var videoIdx = this.getAttribute("data-index");
    var playBtns = this.getElementsByClassName("play-btn");
    var playBtn = playBtns[0];
    var playing = this.getElementsByClassName("paly-ing");
    if (videoPlaying == "0") { //没有正在播放

        //设置当前点击项，添加正在播放等
        this.setAttribute("data-playing", "1");
        videoPlaying = 1;
        playBtn.style.visibility = "hidden";
        var obj = document.createElement("span");
        obj.setAttribute("class", "paly-ing");
        obj.innerHTML = "正在播放";
        playBtn.parentNode.appendChild(obj);

        //设置之前的正在播放项，删除正在播放等
        last_ov.setAttribute("data-playing", "0");
        if (is_first != 1) {
            var objs = last_ov.getElementsByClassName("paly-ing");
            var obj2 = objs[0];
            obj2.parentNode.removeChild(obj2);
            is_first = 0;
        }
        var playBtn2s = last_ov.getElementsByClassName("play-btn");
        var playBtn2 = playBtn2s[0];
        playBtn2.style.visibility = "visible";
        last_ov = this;

        window.WebViewJavascriptBridge.getMessage("PlayVideo", videoIdx, "0");
    }
};

//视频播放
var eles2 = document.getElementsByClassName("other-video");
for (var i = 0; i < eles2.length; i++) {
    eles2[i].addEventListener("click", handler2, false);
}

//视频下载
var eles3 = document.getElementsByClassName("video-desc");
for (var i = 0; i < eles3.length; i++) {
    var buts = eles3[i].getElementsByTagName("i");
    var but = buts[0];
    but.parentNode.setAttribute("href", "javascript:void(0);");
    but.removeEventListener("click", handler2, false);
    but.addEventListener("click", function () {
        var fov = but.parentNode.parentNode.parentNode;
        var videoIdx = fov.getAttribute("data-index");
        window.WebViewJavascriptBridge.getMessage("DownLoadVideo", videoIdx, "1");
    }, false);
}

//翻页视频点击
var eles4 = document.getElementsByClassName("fp-index-btn");
for (var i = 0; i < eles4.length; i++) {
    eles4[i].addEventListener("click", function () {
        var fpVideos = document.body.getElementsByClassName("fp-video");
        var fpVideo = fpVideos[0];
        fpVideo.style.display = "block";
        var Videos = fpVideo.getElementsByClassName("video");
        var Video = Videos[0];
        Video.play();
        document.addEventListener("webkitfullscreenchange", function () {
            if (document.webkitIsFullScreen) {
                window.WebViewJavascriptBridge.fpVideoFullScreen("1");
            }
            else {
                window.WebViewJavascriptBridge.fpVideoFullScreen("0");
            }
        }, false);
    }, false);
}

//翻页评论
var eles5 = document.getElementsByClassName("fp-all");
for (var i = 0; i < eles5.length; i++) {
    eles5[i].addEventListener("click", function () {
        var obj = document.createElement("div");
        obj.innerHTML = "有得到消息";
        document.body.appendChild(obj);
        n();
    }, false);
}