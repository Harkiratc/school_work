$(document).ready(function () {
    $('#latest').each(function () {
        if($(this).children().text() == ""){
            $(this).hide(); 
        }
    });
	$('.courseGrades').each(function(){
	    $(this).hide();
	});
	$('input.reported').each(function () {
	    $(this).attr("checked",true);
	    $(this).attr("disabled", true);
	    $(this).parent('label').css("color","#787878");
	});
	$('.studentInfo').click(function () {
	    $('.courseGrades').each(function () {
	        $(this).slideUp('slow');
	    });
	    var heightvar = $(this).next().height() + 'px';
	    $(this).next().slideToggle("slow");
	    $(this).next().stop().animate({ height:  heightvar }, { queue: false, duration: 500 });
	});
	$('table tr').last(function () {
	    $(this).css("border-bottom","none");
	});
	$('#search input[type=text]').focus(function () {
	    var pos = $(this);
	    pos.keyup(function () {
	        var word = pos.val();
	        timer = setTimeout(function () {
	            if (word == "") {
	                $('.courseGrades').each(function () {
	                    $(this).hide();
	                });
	                $('.studentInfo').each(function () {
	                    $(this).show();
	                    $("#walla > tbody  > tr > td:nth-child(2)").each(function () {
	                        $(this).parent().show();
	                    });
	                });
	            }
	            else{
	                $('.studentInfo').each(function () {
	                    var thisEle = $(this);
	                    var thisCourse = thisEle.children('.centerMainCourse').text().split("<span>Course: </span>");
	                    thisEle.hide();
	                    var check = 0;
	                   /* $("#walla > tbody  > tr > td:nth-child(2)").each(function () {
	                            $(this).parent().show();
	                    });*/
	                    if (thisCourse[0].toLowerCase().indexOf(word) >= 0) {
	                        thisEle.show();
	                    }
	                    else {
	                        $("#walla > tbody  > tr > td:nth-child(2)").each(function () {
	                            if ($(this).text().toLowerCase().indexOf(word) >= 0 && $(this).parent().parent().parent().parent().prev().text() == thisCourse[0]) {
	                                $(this).parent().show();
	                                check = 1;
	                            }
	                            else if ($(this).text().toLowerCase().indexOf(word) < 0 && $(this).parent().parent().parent().parent().prev().text() == thisCourse[0]) {
	                                $(this).parent().hide();
	                            }
	                        });
	                        if (check == 1)
	                            thisEle.show();
	                    }
	                });
	            }
	        }, 500);
	    });
	    $(this).keydown(function () {
	        clearTimeout(timer);
	    });
	});
});
