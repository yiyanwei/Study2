import Vue from 'vue';
import ElementUI from 'element-ui';
import VueResource from 'vue-resource';
import 'element-ui/lib/theme-chalk/index.css';
//import store from './vuex/store.js';
import App from './App.vue';
Vue.use(ElementUI);
Vue.use(VueResource);
Vue.config.productionTip = false;

// Vue.http.interceptors.push((request, next) => {
//   // 请求headers携带参数 
//   //默认
//   var tokenType = store.state.tokenType;
//   if (!tokenType) {
//     tokenType = 'Bearer';
//   }
//   var token = tokenType + ' ' + store.state.token;
//   request.headers.set('Authorization', token);
//   next(function (response) {
//     return response;
//   });
// });
new Vue({
  render: h => h(App)
}).$mount('#app')
