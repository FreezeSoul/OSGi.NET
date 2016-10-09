(function (){

    var websocket;
    var consolePre = "OSGi.NET > "
    $(document).ready(function () {
       $.jGrowl.defaults.closerTemplate = "<div>[ 关闭 ]</div>";
       $("#connectBtn").click(connectBtn);
       $("#commandInput").keyup(function(event){
        if(event.keyCode == 13){
            enterInput(this);
        }
    });
    });

    var connectBtn = function(){
        var btn = this;
        var status = $("#connectBtn").data("connectStatus");
        $(this).isLoading({'class': "icon-loading"});
        $(this).attr('disabled','disabled');
        if(status)
        {
            setTimeout( function(){
                $(btn).isLoading("hide");
                $(btn).removeAttr('disabled');
            }, 5000);
            unConnectServer();
        }
        else
        {
            setTimeout( function(){
                $(btn).isLoading("hide");
                $(btn).removeAttr('disabled');
                var status = $("#connectBtn").data("connectStatus");
                if(!status)
                {
                    writeMessage("与客户端" +  $("#ip").val() + "连接超时");
                    $.get("/v1/client/" + $("#mac").val() + "/离线");
                }
            }, 5000);
            connectServer();
        }
    };

    var enterInput = function(o){
        var status = $("#connectBtn").data("connectStatus");
        if(status)
        {
            var txt = $(o).val();
            $.jGrowl("向客户机发送命令:『" + txt + "』,请稍后...",{ header: '提示' });
            if(txt!="")
                websocket.send(txt);
            $(o).val("");
        }
        else
        {
            $.jGrowl("尚未与客户机建立连接！",{ header: '警告' });
        }
    }

    var connectServer = function(){
        var ip = $("#ip").val();
        var port = $("#port").val();
        var url = 'ws://' + ip + ':' + port;
        websocket = new WebSocket(url);
        websocket.onopen = function(evt) { onOpen(evt) }; 
        websocket.onclose = function(evt) { onClose(evt) }; 
        websocket.onmessage = function(evt) { onMessage(evt) }; 
        websocket.onerror = function(evt) { onError(evt) };
    };

    var unConnectServer = function(){
        if(websocket)
            websocket.close();
    };

    var onOpen = function(evt) { 
        writeMessage("与客户端" +  $("#ip").val() + "建立连接");
        var $btn = $("#connectBtn");
        $btn.text("断开");
        $btn.data("connectStatus",true);
        $btn.isLoading("hide");
        $btn.removeAttr('disabled');
    }; 

    var onClose =  function (evt) { 
        writeMessage("与客户端" +  $("#ip").val() + "断开连接");
        var $btn = $("#connectBtn");
        $btn.text("连接");
        $btn.data("connectStatus",false);
        $btn.isLoading("hide");
        $btn.removeAttr('disabled');
    }; 

    var onMessage = function (evt) { 
        writeMessage(evt.data);
    };

    var onError = function (evt) { 
        unConnectServer();
    };

    var writeMessage = function(txt){
        var newline= "\r\n";
        var consoleLog = $("#outputWin")[0];
        var pre = document.createElement("p");
        pre.style.wordWrap = "break-word";
        pre.innerHTML = "<pre style='background-color:black;color:#cccccc;padding:0px;margin:0px;'>" + consolePre + txt + "</pre>";
        consoleLog.appendChild(pre);
        while (consoleLog.childNodes.length > 100)
        {
          consoleLog.removeChild(consoleLog.firstChild);
        }
        consoleLog.scrollTop = consoleLog.scrollHeight;
    }

}());