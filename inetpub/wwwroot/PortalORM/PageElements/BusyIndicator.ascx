﻿<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="BusyIndicator.ascx.vb" Inherits="CKG.Portal.PageElements.BusyIndicator" %>


<script src="/PortalORM/Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>

<style type="text/css">
    .jqmWindow {
        display: none;
    
        position: fixed;
        top: 35%;
        left: 30%;
    
        margin-left: -300px;
        width: 600px;
    
        background-color: #FFF;
        color: #333;
        border: 1px solid black;
        padding: 20px;
        z-index: 2000000000;
    }

    .jqmOverlay { background-color: #DDD; }

    /* Background iframe styling for IE6. Prevents ActiveX bleed-through (<select> form elements, etc.) */
    * iframe.jqm {position:absolute;top:0;left:0;z-index:-1;
	    width: expression(this.parentNode.offsetWidth+'px');
	    height: expression(this.parentNode.offsetHeight+'px');
    }

    /* Fixed posistioning emulation for IE6
         Star selector used to hide definition from browsers other than IE6
         For valid CSS, use a conditional include instead */
    * html .jqmWindow {
         position: absolute;
         top: expression((document.documentElement.scrollTop || document.body.scrollTop) + Math.round(17 * (document.documentElement.offsetHeight || document.body.clientHeight) / 100) + 'px');
    }
    
    #page-cover {
        display: none;
        position: absolute;
        width: 100%;
        height: 100%;
        background-color: #000;
        z-index: 999;
        top: 0px;
        left: 0px;
    }
</style>

<script type="text/javascript">
    !function (e, t, n) { function o(e, n) { var r = t.createElement(e || "div"), i; for (i in n) r[i] = n[i]; return r } function u(e) { for (var t = 1, n = arguments.length; t < n; t++) e.appendChild(arguments[t]); return e } function f(e, t, n, r) { var o = ["opacity", t, ~ ~(e * 100), n, r].join("-"), u = .01 + n / r * 100, f = Math.max(1 - (1 - e) / t * (100 - u), e), l = s.substring(0, s.indexOf("Animation")).toLowerCase(), c = l && "-" + l + "-" || ""; return i[o] || (a.insertRule("@" + c + "keyframes " + o + "{" + "0%{opacity:" + f + "}" + u + "%{opacity:" + e + "}" + (u + .01) + "%{opacity:1}" + (u + t) % 100 + "%{opacity:" + e + "}" + "100%{opacity:" + f + "}" + "}", a.cssRules.length), i[o] = 1), o } function l(e, t) { var i = e.style, s, o; if (i[t] !== n) return t; t = t.charAt(0).toUpperCase() + t.slice(1); for (o = 0; o < r.length; o++) { s = r[o] + t; if (i[s] !== n) return s } } function c(e, t) { for (var n in t) e.style[l(e, n) || n] = t[n]; return e } function h(e) { for (var t = 1; t < arguments.length; t++) { var r = arguments[t]; for (var i in r) e[i] === n && (e[i] = r[i]) } return e } function p(e) { var t = { x: e.offsetLeft, y: e.offsetTop }; while (e = e.offsetParent) t.x += e.offsetLeft, t.y += e.offsetTop; return t } var r = ["webkit", "Moz", "ms", "O"], i = {}, s, a = function () { var e = o("style", { type: "text/css" }); return u(t.getElementsByTagName("head")[0], e), e.sheet || e.styleSheet } (), d = { lines: 12, length: 7, width: 5, radius: 10, rotate: 0, corners: 1, color: "#000", speed: 1, trail: 100, opacity: .25, fps: 20, zIndex: 2e9, className: "spinner", top: "auto", left: "auto", position: "relative" }, v = function m(e) { if (!this.spin) return new m(e); this.opts = h(e || {}, m.defaults, d) }; v.defaults = {}, h(v.prototype, { spin: function (e) { this.stop(); var t = this, n = t.opts, r = t.el = c(o(0, { className: n.className }), { position: n.position, width: 0, zIndex: n.zIndex }), i = n.radius + n.length + n.width, u, a; e && (e.insertBefore(r, e.firstChild || null), a = p(e), u = p(r), c(r, { left: (n.left == "auto" ? a.x - u.x + (e.offsetWidth >> 1) : parseInt(n.left, 10) + i) + "px", top: (n.top == "auto" ? a.y - u.y + (e.offsetHeight >> 1) : parseInt(n.top, 10) + i) + "px" })), r.setAttribute("aria-role", "progressbar"), t.lines(r, t.opts); if (!s) { var f = 0, l = n.fps, h = l / n.speed, d = (1 - n.opacity) / (h * n.trail / 100), v = h / n.lines; (function m() { f++; for (var e = n.lines; e; e--) { var i = Math.max(1 - (f + e * v) % h * d, n.opacity); t.opacity(r, n.lines - e, i, n) } t.timeout = t.el && setTimeout(m, ~ ~(1e3 / l)) })() } return t }, stop: function () { var e = this.el; return e && (clearTimeout(this.timeout), e.parentNode && e.parentNode.removeChild(e), this.el = n), this }, lines: function (e, t) { function i(e, r) { return c(o(), { position: "absolute", width: t.length + t.width + "px", height: t.width + "px", background: e, boxShadow: r, transformOrigin: "left", transform: "rotate(" + ~ ~(360 / t.lines * n + t.rotate) + "deg) translate(" + t.radius + "px" + ",0)", borderRadius: (t.corners * t.width >> 1) + "px" }) } var n = 0, r; for (; n < t.lines; n++) r = c(o(), { position: "absolute", top: 1 + ~(t.width / 2) + "px", transform: t.hwaccel ? "translate3d(0,0,0)" : "", opacity: t.opacity, animation: s && f(t.opacity, t.trail, n, t.lines) + " " + 1 / t.speed + "s linear infinite" }), t.shadow && u(r, c(i("#000", "0 0 4px #000"), { top: "2px" })), u(e, u(r, i(t.color, "0 0 1px rgba(0,0,0,.1)"))); return e }, opacity: function (e, t, n) { t < e.childNodes.length && (e.childNodes[t].style.opacity = n) } }), function () { function e(e, t) { return o("<" + e + ' xmlns="urn:schemas-microsoft.com:vml" class="spin-vml">', t) } var t = c(o("group"), { behavior: "url(#default#VML)" }); !l(t, "transform") && t.adj ? (a.addRule(".spin-vml", "behavior:url(#default#VML)"), v.prototype.lines = function (t, n) { function s() { return c(e("group", { coordsize: i + " " + i, coordorigin: -r + " " + -r }), { width: i, height: i }) } function l(t, i, o) { u(a, u(c(s(), { rotation: 360 / n.lines * t + "deg", left: ~ ~i }), u(c(e("roundrect", { arcsize: n.corners }), { width: r, height: n.width, left: n.radius, top: -n.width >> 1, filter: o }), e("fill", { color: n.color, opacity: n.opacity }), e("stroke", { opacity: 0 })))) } var r = n.length + n.width, i = 2 * r, o = -(n.width + n.length) * 2 + "px", a = c(s(), { position: "absolute", top: o, left: o }), f; if (n.shadow) for (f = 1; f <= n.lines; f++) l(f, -2, "progid:DXImageTransform.Microsoft.Blur(pixelradius=2,makeshadow=1,shadowopacity=.3)"); for (f = 1; f <= n.lines; f++) l(f); return u(t, a) }, v.prototype.opacity = function (e, t, n, r) { var i = e.firstChild; r = r.shadow && r.lines || 0, i && t + r < i.childNodes.length && (i = i.childNodes[t + r], i = i && i.firstChild, i = i && i.firstChild, i && (i.opacity = n)) }) : s = l(t, "animation") } (), typeof define == "function" && define.amd ? define(function () { return v }) : e.Spinner = v } (window, document);    
</script>

<script type="text/javascript">
    $(document).ready(function () {
        // Setup the busy indicator (2012, Matthias Jenzen)
        $('#ajaxBusy').css({
            display: "none",
            margin: "0px",
            paddingLeft: "0px",
            paddingRight: "0px",
            paddingTop: "0px",
            paddingBottom: "0px",
            width: "auto"
        });
        
        $('#ajaxBusy').hide();

        $('body').append('<div id="page-cover">Test</div>');
    });

    function showBusyHint(hintText) {
        $('#busyIndicatorHint').html(hintText);
        $('#ajaxBusy').show();

        showSpinner();
        $("#page-cover").show().css("opacity", 0.25);
    }

    function showSpinner() {
        var opts = {
            lines: 11, // The number of lines to draw
            length: 4, // The length of each line
            width: 2, // The line thickness
            radius: 3, // The radius of the inner circle
            corners: 1, // Corner roundness (0..1)
            rotate: 0, // The rotation offset
            color: '#000', // #rgb or #rrggbb
            speed: 1.2, // Rounds per second
            trail: 88, // Afterglow percentage
            shadow: false, // Whether to render a shadow
            hwaccel: false, // Whether to use hardware acceleration
            className: 'spinner', // The CSS class to assign to the spinner
            zIndex: 2e9, // The z-index (defaults to 2000000000)
            top: '-11px', // Top position relative to parent in px
            left: '-5px' // Left position relative to parent in px
        };
        var target = document.getElementById('busyIndicatorAnimationTd');
        var spinner = new Spinner(opts).spin(target);
        target.appendChild(spinner.el);
    }
    function Show_BusyBox1() {
        showBusyHint('Ihr Vorgang wird bearbeitet. Dies kann einige Zeit in Anspruch nehmen.');
    }

</script>

<div id="ajaxBusy" class="jqmWindow">
    <table style="padding: 10px; margin: 10px;">
        <tr>
            <td id="busyIndicatorAnimationTd" style="vert-align: middle">
            </td>
            
            <td style="vert-align: middle">
                <span id="busyIndicatorHint" style="margin: 0px; padding-left: 20px;">
                    Bitte einen Moment Geduld ...
                </span>
            </td>
        </tr>
    </table>
</div>