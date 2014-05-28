/**
* Determines the strength of a given password based on
* frequency of occurrence of lowercase, uppercase,
* numbers and the special characters passed via the spc_chars
* argument.  The more even the spread of occurrences, the
* stronger the password.
*
* This class contains the following public parameters:
*   'lcase_count'    : lowercase occurrence count.
*   'ucase_count'    : uppercase occurrence count.
*   'num_count'      : number occurrence count.
*   'schar_count'    : special character occurrence count.
*   'length'         : length of password string.
*   'strength'       : strength value of password.
*   'verdict'        : textual strength indication
*                      ['weak', 'medium', 'strong'].
*
* @param string arg_password  The password
* @param string arg_spc_chars A string of special characters
*     to search for in the password. By making this an
*     argument, the range of special characters can be
*     controlled  externally.
* @return string The verdict as 'Weak'|'Medium'|'Strong'
*/
function Password(arg_password, arg_spc_chars) {
    var password = arg_password;
    var spc_chars = arg_spc_chars;
    this.lcase_count = 0;
    this.ucase_count =0;
    this.num_count = 0;
    this.schar_count = 0;
    this.length = 0;
    this.strength = 0;
    this.runs_score = 0;
    this.verdict = '';

    // These numbers are just guesses on my part (and not
    // all that educated, either ;) Adjust accordingly.
    var verdict_conv = { 'weak': 2.7, 'medium': 53, 'strong': 150 };

    // These are weighting factors.  I figure that including
    // numbers is a little better than including uppercase
    // because numbers probably are not vulnerable to
    // dictionary searches, and including special chars is
    // even better.  These factors provide yet another
    // dimension.  Again, there are only guesses.
    var flc = 1.0;  // lowercase factor
    var fuc = 1.0;  // uppercase factor
    var fnm = 1.3;  // number factor
    var fsc = 1.5;  // special char factor

    this.getStrength = function() {
        if ((this.run_score = this.detectRuns()) <= 1) {
            return "Very weak";
        }

        var regex_sc = new RegExp('[' + spc_chars + ']', 'g');

        this.lcase_count = password.match(/[a-z]/g);
        this.lcase_count = (this.lcase_count) ? this.lcase_count.length : 0;
        this.ucase_count = password.match(/[A-Z]/g);
        this.ucase_count = (this.ucase_count) ? this.ucase_count.length : 0;
        this.num_count = password.match(/[0-9]/g);
        this.num_count = (this.num_count) ? this.num_count.length : 0;
        this.schar_count = password.match(regex_sc);
        this.schar_count = (this.schar_count) ? this.schar_count.length : 0;
        this.length = password.length;

        var avg = this.length / 4;

        // I'm dividing by (avg + 1) to linearize the strength a bit.
        // To get a result that ranges from 0 to 1, divide 
        // by Math.pow(avg + 1, 4)
        this.strength = ((this.lcase_count * flc + 1) *
                         (this.ucase_count * fuc + 1) *
                         (this.num_count * fnm + 1) *
                         (this.schar_count * fsc + 1)) / (avg + 1);

        if (this.strength > verdict_conv.strong)
            this.verdict = 'Strong';
        else if (this.strength > verdict_conv.medium)
            this.verdict = 'Medium';
        else if (this.strength > verdict_conv.weak)
            this.verdict = 'Weak';
        else
            this.verdict = "Forget it!";

        return this.verdict;
    }

    // This is basically an edge detector with a 'rectified' (or
    // absolute zero) result.  The difference of adjacent equivalent 
    // char values is zero.  The greater the difference, the higher
    // the result.  'aaaaa' sums to 0. 'abcde' sums to 1.  'acegi'
    // sums to 2, etc.  'aaazz', which has a sharp edge, sums to  
    // 6.25.  Any thing 1 or below is a run, and should be considered
    // weak.
    this.detectRuns = function() {
        var parts = password.split('');
        var ords = new Array();
        for (i in parts) {
            ords[i] = parts[i].charCodeAt(0);
        }

        var accum = 0;
        var lasti = ords.length - 1

        for (var i = 0; i < lasti; ++i) {
            accum += Math.abs(ords[i] - ords[i + 1]);
        }

        return accum / lasti;
    }


    this.toString = function() {
        return 'lcase: ' + this.lcase_count +
               ' -- ucase: ' + this.ucase_count +
               ' -- nums: ' + this.num_count +
               ' -- schar: ' + this.schar_count +
               ' -- strength: ' + this.strength +
               ' -- verdict: ' + this.verdict;
    }
}

var required_lcase_count = 0;
var required_ucase_count = 0;
var required_num_count = 0;
var required_schar_count = 0;
var required_length = 0;
function checkPassword(length,lcase,ucase,num,special) {
    var special_chars = "!§$%&/()=?#*<>@";

    required_length = length
    required_lcase_count = lcase
    required_ucase_count = ucase
    required_num_count = num
    required_schar_count = special

    var pw = new Password(document.getElementById('txtNewPwd').value,
                           special_chars);

    var verdict = pw.getStrength();
    var hint = '';
    var Status = 0;
    //  Großbuchstaben
    element = document.getElementById("divUCase");
    element2 = document.getElementById("spanUCase");
    if (pw.ucase_count >= required_ucase_count) {
        hint = "erfüllt"
        element.style.color = "#30A000"
        element2.style.color = "#30A000"
    }
    else {
        hint = "nicht erfüllt"
        element.style.color = "#B70000"
        element2.style.color = "#B70000"
    }
      element.innerHTML = hint;


    //  Zahlen
      element = document.getElementById("divNumeric");
      element2 = document.getElementById("spanNumeric");
    if (pw.num_count >= required_num_count) {
        hint = "erfüllt"
        element.style.color = "#30A000"
        element2.style.color = "#30A000"
    }
    else {
        hint = "nicht erfüllt"
        element.style.color = "#B70000"
        element2.style.color = "#B70000"

    }
     element.innerHTML = hint;


     //  Sonderzeichen
     element = document.getElementById("divSpecial");
     element2 = document.getElementById("spanSpecial");
    if (pw.schar_count >= required_schar_count) {
        hint = "erfüllt"
        element.style.color = "#30A000"
        element2.style.color = "#30A000"
    }
    else {
        hint = "nicht erfüllt"
        element.style.color = "#B70000"
        element2.style.color = "#B70000"
    }
   
    element.innerHTML = hint;

//    if (pw.run_score <= 1) hint += "Avoid runs (e.g. 'aaaa', 'efghi', '1234'). ";

    element = document.getElementById("divLength");
    element2 = document.getElementById("spanLength");
    pswdLen = document.getElementById('txtNewPwd').value.length
    if (pswdLen  >= required_length) {
        element.innerHTML = "erfüllt";
        element.style.color = "#30A000"
        element2.style.color = "#30A000"
       
    }
    else {
        element.innerHTML = required_length + " jetzt noch " + (required_length - pswdLen) + " Zeichen";
        element.style.color = "#B70000"
        element2.style.color = "#B70000"
    }
}
