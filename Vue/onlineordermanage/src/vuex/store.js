import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

//vuex 数据对象
var state={
    token:localStorage.getItem('token')?localStorage.getItem('token'):'',
    tokenType:localStorage.getItem('tokenType')?localStorage.getItem('tokenType'):''
};

//vuex 方法对象
var mutations={
    changeLogin(state,user){
        localStorage.setItem('token',user.access_token);
        localStorage.setItem('tokenType',user.token_type);
    }
};

//定义vuex对象
const store = new Vuex.Store({
    state,
    mutations
});

//暴露vuex的对象store
export default store;