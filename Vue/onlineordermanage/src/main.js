import Vue from 'vue';
import ElementUI from 'element-ui';
import VueResource from 'vue-resource';
import 'element-ui/lib/theme-chalk/index.css';
import App from './App.vue';
import VueRouter from 'vue-router';
Vue.use(ElementUI);
Vue.use(VueResource);
Vue.use(VueRouter);
Vue.config.productionTip = false;

//1、引入组件，用来配置路由
import CategoryList from './components/procategory/CategoryList.vue';
import ProductList from './components/product/ProductList.vue';

//2、配置路由
const routes=[
  {
    path:'/procategorylist',component:CategoryList
  },
  {
    path:'/prolist',component:ProductList
  }
];

//3、实例化路由
const router =new VueRouter({
  routes
});

//4、挂载路由

new Vue({
  render: h => h(App),
  router
}).$mount('#app')
