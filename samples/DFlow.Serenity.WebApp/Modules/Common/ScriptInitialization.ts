/// <reference path="../Common/Helpers/LanguageList.ts" />

namespace SereneDemo.ScriptInitialization {
    Q.Config.responsiveDialogs = true;
    Q.Config.rootNamespaces.push('SereneDemo');
    Serenity.EntityDialog.defaultLanguageList = LanguageList.getValue;
}