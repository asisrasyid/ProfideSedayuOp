/**
 * Config
 * -------------------------------------------------------------------------------------
 * ! IMPORTANT: Make sure you clear the browser local storage In order to see the config changes in the template.
 * ! To clear local storage: (https://www.leadshook.com/help/how-to-clear-local-storage-in-google-chrome-browser/).
 */

'use strict';

// JS global variables
window.config = {
  colors: {
    primary: '#8c57ff',
    secondary: '#8a8d93',
    success: '#56ca00',
    info: '#16b1ff',
    warning: '#ffb400',
    danger: '#ff4c51',
    dark: '#4b4b4b',
    black: '#2e263d',
    white: '#fff',
    cardColor: '#fff',
    bodyBg: '#f4f5fa',
    bodyColor: '#6D6777',
    headingColor: '#433C50',
    textMuted: '#ABA8B1',
    borderColor: '#E6E5E8',
    chartBgColor: '#F0F2F8'
  },
  colors_label: {
    primary: '#8c57ff1f',
    secondary: '#8a8d931f',
    success: '#56ca001f',
    info: '#16b1ff1f',
    warning: '#ffb4001f',
    danger: '#ff4c511f',
    dark: '#4b4b4b1f'
  },
  colors_dark: {
    cardColor: '#312d4b',
    bodyBg: '#28243d',
    bodyColor: '#B0ACC7',
    headingColor: '#D5D1EA',
    textMuted: '#7A7692',
    borderColor: '#474360',
    chartBgColor: '#474360'
  },
  enableMenuLocalStorage: true // Enable menu state with local storage support
};

window.assetsPath = document.documentElement.getAttribute('data-assets-path');
window.templateName = document.documentElement.getAttribute('data-template');
window.rtlSupport = true; // set true for rtl support (rtl + ltr), false for ltr only.

/**
 * TemplateCustomizer
 * ! You must use(include) template-customizer.js to use TemplateCustomizer settings
 * -----------------------------------------------------------------------------------------------
 */

// To use more themes, just push it to THEMES object.

/* TemplateCustomizer.THEMES.push({
  name: 'theme-raspberry',
  title: 'Raspberry'
}); */

// To add more languages, just push it to LANGUAGES object.
/*
TemplateCustomizer.LANGUAGES.fr = { ... };
*/

/**
 * TemplateCustomizer settings
 * -------------------------------------------------------------------------------------
 * cssPath: Core CSS file path
 * themesPath: Theme CSS file path
 * displayCustomizer: true(Show customizer), false(Hide customizer)
 * lang: To set default language, Add more langues and set default. Fallback language is 'en'
 * controls: [ 'rtl', 'style', 'headerType', 'contentLayout', 'layoutCollapsed', 'layoutNavbarOptions', 'themes' ] | Show/Hide customizer controls
 * defaultTheme: 0(Default), 1(Bordered), 2(Semi Dark)
 * defaultStyle: 'light', 'dark', 'system' (Mode)
 * defaultTextDir: 'ltr', 'rtl' (rtlSupport must be true for rtl mode)
 * defaultContentLayout: 'compact', 'wide' (compact=container-xxl, wide=container-fluid)
 * defaultHeaderType: 'static', 'fixed' (for horizontal layout only)
 * defaultMenuCollapsed: true, false (For vertical layout only)
 * defaultNavbarType: 'sticky', 'static', 'hidden' (For vertical layout only)
 * defaultFooterFixed: true, false (For vertical layout only)
 * defaultShowDropdownOnHover : true, false (for horizontal layout only)
 */

if (typeof TemplateCustomizer !== 'undefined') {
  window.templateCustomizer = new TemplateCustomizer({
    cssPath: assetsPath + 'vendor/css' + (rtlSupport ? '/rtl' : '') + '/',
    themesPath: assetsPath + 'vendor/css' + (rtlSupport ? '/rtl' : '') + '/',
    displayCustomizer: true,
    lang: localStorage.getItem('templateCustomizer-' + templateName + '--Lang') || 'en', // Set default language here
    // defaultTheme: 2,
    // defaultStyle: 'system',
    // defaultTextDir: 'rtl',
    // defaultContentLayout: 'wide',
    // defaultHeaderType: 'static',
    // defaultMenuCollapsed: true,
    // defaultNavbarType: 'sticky',
    // defaultFooterFixed: false,
    // defaultShowDropdownOnHover: false,
    controls: ['rtl', 'style', 'headerType', 'contentLayout', 'layoutCollapsed', 'layoutNavbarOptions', 'themes']
  });
}
