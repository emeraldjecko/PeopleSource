/*
 * jQuery UI Throbber 0.1
 *
 * Copyright (c) 2009 Jeremy Lea <reg@openpave.org>
 * Dual licensed under the MIT and GPL licenses.
 *
 * http://docs.jquery.com/Licensing
 *
 * Based loosely on the Raphael spinner demo.
 */
(function ($) {

    $.widget("ui.throbber", {
        _create: function () {

            var o = this.options, e = this.element, a, i, w, tmp;

            var NS = "http://www.w3.org/2000/svg";
            this._wrapper = this.element[0].insertBefore(
                document.createElementNS(NS, "svg"), this.element[0].firstChild);
            tmp = this._wrapper;
            tmp.setAttribute("viewBox", "-100 -100 200 200");
            tmp.style.position = "absolute";
            tmp.style.width = "100%";
            tmp.style.height = "100%";
            tmp = this._wrapper.appendChild(
                document.createElementNS(NS, "rect"));
            tmp.setAttribute("x", "-100px");
            tmp.setAttribute("y", "-100px");
            tmp.setAttribute("width", "200px");
            tmp.setAttribute("height", "200px");
            tmp.setAttribute("rx", o.bgcorner);
            tmp.setAttribute("fill", o.bgcolor);
            tmp.setAttribute("fill-opacity", o.bgopacity);
            tmp = this._wrapper.appendChild(document.createElementNS(NS, "g"));
            tmp.setAttribute("fill", o.fgcolor);
            for (i = 0; i < o.segments; ++i) {
                a = -i * 360 / o.segments;
                tmp = this._wrapper.childNodes[1]
                    .appendChild(document.createElementNS(NS, "path"));
                tmp.setAttribute("d", o.path(i, o.segments));
                tmp.setAttribute("fill-opacity", o.opacity(i, o.segments));
                tmp.setAttribute("transform", "rotate(" + a + ")");
            }
        },
        destroy: function () {
            this._wrapper.remove();

            $.widget.prototype.destroy.apply(this, arguments);
        },
        reset: function () {
            this._pos = -1;
            this._update();
        },

        _timer: false,
        _setOption: function (key, value) {
            var self = this, o = this.options;

            this.options[key] = value;
            if (key == "disabled") {
                if (!value) {
                    if (this._timer) {
                        this._timer = clearInterval(this._timer);
                    }
                    this._timer = setInterval(function () {
                        self._update();
                    }, 1000 / this.options.segments / this.options.speed);
                    if (o.show) {
                        o.show.call(this.element);
                    }
                } else {
                    if (!o.hide) {
                        o.hide = function (callback) { callback(); };
                    }
                    o.hide.call(this.element, function () {
                        if (self._timer) {
                            self._timer = clearInterval(self._timer);
                        }
                    });
                }
            }
        },
        _pos: 0,
        _update: function () {
            var o = this.options;
            this._pos = (this._pos + 1) % o.segments;
            var a = this._pos * 360 / o.segments;

            this._wrapper.childNodes[1]
                .setAttribute("transform", "rotate(" + a + ")");

        }
    });
    $.ui.throbber.prototype.options = {
        //    bgcolor: "#90BC00",
        bgcolor: "#F",
        bgopacity: 0,
        bgcorner: 10,
        fgcolor: "#000",
        segments: 12,
        path: function (i, segments) {
            return "M 40,8 c -4,0 -8,-4 -8,-8 c 0,-4 4,-8 8,-8 " +
                "l " + (30 - i) + ",0 c 4,0 8,4 8,8 c 0,4 -4,8 -8,8 " +
                "l -" + (30 - i) + ",0 z";
        },
        opacity: function (i, segments) {
            return Math.cos(Math.PI / 2 * (i % segments) / segments);
        },
        speed: 1
    };

})(jQuery);