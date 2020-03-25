var contextMenus = {};

contextMenus.searchDictionary = 
    chrome.contextMenus.create(
        {"title":"Потърсете в архаичния речник",
        "contexts" : ["page", "frame"]
        },
        function (){
            if(chrome.runtime.lastError){
                console.error(chrome.runtime.lastError.message);
            }
        }
    );

chrome.contextMenus.onClicked.addListener(contextMenuHandler);

function contextMenuHandler(info){
    if(info.menuItemId===contextMenus.searchDictionary){
        (function(){
            s=document.selection?document.selection.createRange().text:window.getSelection?window.getSelection().toString():document.getSelection?document.getSelection():'';
            if(s==''){
                s=prompt('Коя дума искате да потърсите?','');
            }if(s){
                window.open('http://archaismdictionary.bg/dictionary.php?word='+s,'_blank')
            };
        })()
    }
}

searchWord = function(word){
    var query = word.selectionText;
    window.open('http://archaismdictionary.bg/dictionary.php?word='+query);
}

chrome.contextMenus.create({
title: "Сканиране на архаичната дума",
contexts:["selection", "editable"],
onclick: searchWord
});