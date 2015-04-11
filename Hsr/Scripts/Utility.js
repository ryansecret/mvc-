function extendDeep(parent, child) {
    var i,
        toStr = Object.prototype.toString,
        astr = "[object Array]";

    child = child || {};

    for (i in parent) {
        if (parent.hasOwnProperty(i)) {
            if (typeof parent[i] === 'object') {
                child[i] = (toStr.call(parent[i]) === astr) ? [] : {};
                extendDeep(parent[i], child[i]);
            } else {
                child[i] = parent[i];
            }
        }
    }
    return child;
}

var docElem = window.document.documentElement;
function getViewportH() {
    var client = docElem['clientHeight'],
        inner = window['innerHeight'];

    if (client < inner)
        return inner;
    else
        return client;
}

function scrollY() {
    return window.pageYOffset || docElem.scrollTop;
}
function getOffset(el) {
    var offsetTop = 0, offsetLeft = 0;
    do {
        if (!isNaN(el.offsetTop)) {
            offsetTop += el.offsetTop;
        }
        if (!isNaN(el.offsetLeft)) {
            offsetLeft += el.offsetLeft;
        }
    } while (el = el.offsetParent)

    return {
        top: offsetTop,
        left: offsetLeft
    };
}
function inViewport(el, h) {
    var elH = el.offsetHeight,
        scrolled = scrollY(),
        viewed = scrolled + getViewportH(),
        elTop = getOffset(el).top,
        elBottom = elTop + elH,
        // if 0, the element is considered in the viewport as soon as it enters.
        // if 1, the element is considered in the viewport only when it's fully inside
        // value in percentage (1 >= h >= 0)
        h = h || 0;

    return (elTop + elH * h) <= viewed && (elBottom) >= scrolled;
}
function mix() {
    var arg, prop, child = {};
    for (arg = 0; arg < arguments.length; arg += 1) {
        for (prop in arguments[arg]) {
            if (arguments[arg].hasOwnProperty(prop)) {
                child[prop] = arguments[arg][prop];
            }
        }
    }
    return child;
}

if (typeof Function.prototype.method !== "function") {
    Function.prototype.method = function (name, implementation) {
        this.prototype[name] = implementation;
        return this;
    };
}