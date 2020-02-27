import Vue from 'vue';
import VueResource from 'vue-resource';
import store from '../../vuex/store.js';
Vue.use(VueResource);

Vue.http.interceptors.push((request, next) => {
  //request.credentials = true; // 接口每次请求会跨域携带cookie
  //request.method= 'POST'; // 请求方式（get,post）
  //设置token值
  var token = 'Bearer ' + store.state.Authorization;
  request.headers.set('token',token);
  next(function (response) {
    return response;

  });
});

var http ={
  get(url){
    Vue.http.get(url).then(response=>{
      
    },error=>{

    });
  }
};

export default http;

// /*get方式请求数据*/
// export function get(url, sucess, err) {
//   // GET /someUrl
//   this.$http.get(url).then((resInfo) => {
//     // get body data
//     sucess(resInfo.body);
//   }, (errInfo) => {
//     err(errInfo);
//   });
// }