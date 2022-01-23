/**
 * jQuery DOB Picker
 * Website: https://github.com/tyea/dobpicker
 * Version: 1.0
 * Author: Tom Yeadon
 * License: BSD 3-Clause
 */

jQuery.extend({

	dobPicker: function(params) {

	   
		// set the defaults		
		if (typeof(params.dayDefault) === 'undefined') params.dayDefault = 'Day';
		if (typeof(params.monthDefault) === 'undefined') params.monthDefault = 'Month';
		if (typeof(params.yearDefault) === 'undefined') params.yearDefault = 'Year';
		if (typeof(params.minimumAge) === 'undefined') params.minimumAge = 12;
		if (typeof(params.maximumAge) === 'undefined') params.maximumAge = 80;

		// set the default messages		
		//$(params.daySelector).append('<option value="">' + params.dayDefault + '</option>');
		//$(params.monthSelector).append('<option value="">' + params.monthDefault + '</option>');
		//$(params.yearSelector).append('<option value="">' + params.yearDefault + '</option>');

	    // populate the day select

		for (i = 1; i <= 31; i++) {
		
			
			    $(params.daySelector).append('<option value="' + i + '">' + i + '</option>');
			

		}
        
		

		// populate the month select		
		var months = [
			"January",
			"February",
			"March",
			"April",
			"May",
			"June",
			"July",
			"August",
			"September",
			"October",
			"November",
			"December"
		];
			
		for (i = 1; i <= 12; i++) {
			
			$(params.monthSelector).append('<option value="' +i + '">' + months[i - 1] + '</option>');
		}

		// populate the year select
		var date = new Date();
		var year = date.getFullYear();
		var min =  params.minimumAge;
		var max = params.maximumAge;
		
		for (i = max; i>=min; i--) {
			$(params.yearSelector).append('<option value="' + i + '">' + i + '</option>');
		}
		
		// do the logic for the day select
		$(params.daySelector).change(function() {
			
		
			$(params.monthSelector)[0].selectedIndex = 0;
			$(params.yearSelector)[0].selectedIndex = 0;
			$(params.yearSelector + ' option').removeAttr('disabled');
			
			if ($(params.daySelector).val() >= 1 && $(params.daySelector).val() <= 29) {
			
				$(params.monthSelector + ' option').removeAttr('disabled');
				
			} else if ($(params.daySelector).val() == 30) {
			
				$(params.monthSelector + ' option').removeAttr('disabled');
				$(params.monthSelector + ' option[value="2"]').attr('disabled', 'disabled');
				
			} else if($(params.daySelector).val() == 31) {
			
				$(params.monthSelector + ' option').removeAttr('disabled');
				$(params.monthSelector + ' option[value="2"]').attr('disabled', 'disabled');
				$(params.monthSelector + ' option[value="4"]').attr('disabled', 'disabled');
				$(params.monthSelector + ' option[value="6"]').attr('disabled', 'disabled');
				$(params.monthSelector + ' option[value="9"]').attr('disabled', 'disabled');
				$(params.monthSelector + ' option[value="11"]').attr('disabled', 'disabled');
				
			}
			
		});
		
		// do the logic for the month select
		$(params.monthSelector).change(function() {
			
			$(params.yearSelector)[0].selectedIndex = 0;
			$(params.yearSelector + ' option').removeAttr('disabled');
			
			if ($(params.daySelector).val() == 29 && $(params.monthSelector).val() == '2') {
			
				$(params.yearSelector + ' option').each(function(index) {
					if (index !== 0) {
						var year = $(this).attr('value');
						var leap = !((year % 4) || (!(year % 100) && (year % 400)));
						if (leap === false) {
							$(this).attr('disabled', 'disabled');
						}
					}
				});
				
			}
			
		});
		

		//$(params.daySelector).val(params.setDay).trigger('change');
		//$(params.monthSelector).val(params.monthDefault).trigger('change');
		//$(params.yearSelector).val(params.yearDefault);
	}
	
});
