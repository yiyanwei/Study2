import Vue from 'vue';
import Vuex from 'vuex';
Vue.use(Vuex);

//vuex 数据对象
var state={
    Authorization:localStorage.getItem('Authorization')?localStorage.getItem('Authorization'):''
};

//vuex 方法对象
var mutations={
    changeLogin(user){
        state.Authorization = user.Authorization;
        localStorage.setItem('Authorization',user.Authorization);
    }
};

//定义vuex对象
const store = new Vuex.Store({
    state,
    mutations
});

//暴露vuex的对象store
export default store;