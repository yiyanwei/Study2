import Vue from 'vue';
import VueResource from 'vue-resource';
import store from '../../vuex/store.js';
Vue.use(VueResource);

//http拦截器
Vue.http.interceptors.push((request, next) => {
  // 请求headers携带参数 
  //默认
  var tokenType = store.state.tokenType;
  if (!tokenType) {
    tokenType = 'Bearer';
  }
  var token = tokenType + ' ' + store.state.token;
  request.headers.set('Authorization', token);
  next(function (response) {
    //判断服务器是否有返回结果
    if (response && response.data) {
      var data = response.data;
      //如果是没有权限，跳转到登录页面
      if (!data.success && data.errCode == 10001) {
        login({
          account: "13382984883",
          password: "123456",
          loginOrigin: 2
        });
      }
    }
    return response;
  });
});


/*用户登录*/
const loginUrl = '/User/UserLogin';
export function login(data) {
  post(loginUrl, data, successInfo => {
    //设置token
    store.commit('changeLogin', successInfo.data);
  }, errInfo => {
    console.log(errInfo);
  });
}

/**
	* 把json对象拆成url参数格式的方法
	* @data 需要处理的json对象
	*/
function getParam(data) {
  let url = '';
  for (var k in data) {
    let value = data[k] !== undefined ? data[k] : '';
    url += `&${k}=${encodeURIComponent(value)}`
  }
  return url ? url.substring(1) : ''
}
//最终获取带参数的url方法
function newUrl(url, data) {
  //看原始url地址中开头是否带?，然后拼接处理好的参数
  return url += (url.indexOf('?') < 0 ? '?' : '') + getParam(data)
}

//根目录
const rootApi = "http://localhost:8888/api";

/*get方式请求数据*/
function get(url, data, success, error) {
  //将json对象序列化成url的param
  var api = '';
  if (data) {
    api = rootApi + newUrl(url, JSON.parse(data));
  }
  else {
    api = rootApi + url;
  }
  Vue.http.get(api).then((resInfo) => {
    if (success && typeof success === "function") {
      success(resInfo.data);
    }
  }, (errInfo) => {
    if (error && typeof error === "function") {
      error(errInfo);
    }
  });
}

/*post方式请求数据*/
function post(url, data, success, error) {
  var api = rootApi + url;
  Vue.http.post(api, JSON.stringify(data)).then((resInfo) => {
    if (success && typeof success === "function") {
      success(resInfo.data);
    }
  }, (errInfo) => {
    if (error && typeof error === "function") {
      error(errInfo);
    }
  });
}

/*put方式请求数据*/
function put(url,data,success,error){
  var api = rootApi + url;
  Vue.http.put(api, JSON.stringify(data)).then((resInfo) => {
    if (success && typeof success === "function") {
      success(resInfo.data);
    }
  }, (errInfo) => {
    if (error && typeof error === "function") {
      error(errInfo);
    }
  });
}

/* delete请求 */
function del(url,data,success,error){
  var api = rootApi + url;
  Vue.http.delete(api, data).then((resInfo) => {
    if (success && typeof success === "function") {
      success(resInfo.data);
    }
  }, (errInfo) => {
    if (error && typeof error === "function") {
      error(errInfo);
    }
  });
}

//暴露几个函数
export default {
  login,
  get,
  post,
  put,
  del
}