window.history.forward(1);

function openUsers() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayUsers.aspx', 'Search User', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openUsers?")
        parent.location.reload(true);
        return true
    }
} 

function openRMs() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayRM.aspx', 'Search RM', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openRMs?")
        parent.location.reload(true);
        return true
    }
} 

function openAMs() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayAM.aspx', 'Search AM', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openAMs?")
        parent.location.reload(true);
        return true
    }
} 

function openBMs() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayBM.aspx', 'Search BM', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openBMs?")
        parent.location.reload(true);
        return true
    }
} 

function openBranchType() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayBranchType.aspx', 'Search Branch Type', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openBranchType?")
        parent.location.reload(true);
        return true
    }
}

function openHO() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayHO.aspx', 'Search HO', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openHO?")
        parent.location.reload(true);
        return true
    }
}

function openIntAccess() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayIntAccess.aspx', 'Search Internet Access', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openIntAccess?")
        parent.location.reload(true);
        return true
    }
} 

function openISP() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayISP.aspx', 'Search ISP', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openISP?")
        parent.location.reload(true);
        return true
    }
} 

function openOS() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayOS.aspx', 'Search OS', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openOS?")
        parent.location.reload(true);
        return true
    }
} 

function openBCStatus() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayBranchStatus.aspx', 'Search Branch Status', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openBCStatus?")
        parent.location.reload(true);
        return true
    }
} 

function openBranch() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayBranch.aspx', 'Search Branch', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openBranch?")
        parent.location.reload(true);
        return true
    }
} 

function openArea() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayArea.aspx', 'Search Area', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openArea?")
        parent.location.reload(true);
        return true
    }
} 

function openRegion() {
    userswindow = dhtmlmodal.open('EmailBox', 'iframe', '../windowpage/displayRegion.aspx', 'Search Region', 'width=350px,height=200px,center=1,resize=0,scrolling=0')
    userswindow.onclose = function() { //Run custom code when window is being closed (return false to cancel action):
        //return window.confirm("openRegion?")
        parent.location.reload(true);
        return true
    }
} 

function onTextBoxUpdate(evt) {
    var textBoxID = evt.source.textBoxID;
    var findComma = evt.selMenuItem.label.indexOf(',');
    if (findComma > 0) {
        var index = findComma + 2;
        var eadd = evt.selMenuItem.label.substring(index)
        document.getElementById(textBoxID).value = eadd;
    }
    //Cancel default behaviour - setting of textbox value to selMenuItem.label
    evt.preventDefault();
}

function PrintThisPage() {
    var sOption = "toolbar=yes,location=no,directories=yes,menubar=yes,";
    sOption += "scrollbars=yes,width=750,height=600,left=100,top=25";
    var sWinHTML = document.getElementById('main-copy').innerHTML;

    var winprint = window.open("", "", 'scrollbars=1,width=650,height=500,menubar=1,resizable=1');
    winprint.document.open();
    winprint.document.write('<html><link href="../CSS/StylePrintPreview.css" rel="stylesheet"><body>');
    winprint.document.write('<body><img src="../Images/MLBig.gif" alt="Logo" style="margin-right:10px;" />');
    winprint.document.write(sWinHTML);
    winprint.document.write('</body></html>');
    winprint.document.close();
    winprint.focus();
    winprint.print();
}       
       
      

