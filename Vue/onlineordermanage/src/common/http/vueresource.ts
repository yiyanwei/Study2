import VueResource from 'vue-resource';

/*get方式请求数据*/
export function get(url,sucess,err){
  // GET /someUrl
  this.$http.get(url).then((resInfo) => {
    // get body data
    sucess(resInfo.body);
  }, (errInfo) => {
    err(errInfo);
  });
}