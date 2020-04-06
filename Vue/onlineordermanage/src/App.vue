<template>
  <div id="app">
    <MenuList />
  </div>
</template>

<script>
// import AddPro from "./components/product/AddPro.vue";
// import AddCategory from "./components/procategory/AddCategory.vue";
// import CategoryList from "./components/procategory/CategoryList.vue";
// import ProInfoList from "./components/product/ProductList.vue";
import MenuList from "./components/menu/munulist.vue";
// import SignalRCom from "./components/SignalR.vue";

export default {
  name: "App",
  components: {
    // SignalRCom
    MenuList
    // CategoryList,
    // ProInfoList
  }
};
</script>

<style>
  body
  {
    margin:0px;
  }
</style>
