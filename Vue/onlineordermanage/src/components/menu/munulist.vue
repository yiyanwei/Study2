<template>
  <div>
    <el-container style="border: 1px solid #eee">
      <el-aside width="250px">
        <el-menu router class="el-menu-vertical-demo" :default-active="this.$route.path" mode="vertical" >
          <template v-for="item in menus">
            <el-submenu v-if="item.second" :key="item.id" v-show="item.second" index="item.index">
              <template slot="title">{{ item.name }}</template>
              <el-menu-item
                v-for="(sec) in item.second"
                :key="sec.id"
                :index="sec.index"
              >{{ sec.name }}</el-menu-item>
            </el-submenu>
          </template>
          <template v-for="item in menus">
            <el-menu-item
              v-if="!item.second"
              v-show="!item.second"
              :key="item.id"
              :index="item.index"
            >{{ item.name }}</el-menu-item>
          </template>
        </el-menu>
      </el-aside>
      <el-main>
        <router-view class="routerView"></router-view>
      </el-main>
    </el-container>
  </div>
</template>
<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
  min-width: 200px;
  min-height: 400px;
  height: 100%;
}
</style>

<script>
export default {
  data() {
    return {
      menus: [
        {
          id: "1",
          index: "/prolist",
          name: "产品管理",
          second: [
            { id: 2, index: "/procategorylist", name: "产品分类管理" },
            { id: 3, index: "/prolist", name: "产品管理" }
          ]
        },
        {
          id:"4",
          index:"/suplist",
          name:"供应商管理",
          second: [
            { id: 4, index: "/suplist", name: "供应商管理" }
            // ,
            // { id: 3, index: "/prolist", name: "产品管理" }
          ]
        }
      ],
      isCollapse: true
    };
  },
  methods: {
    handleOpen(key, keyPath) {
      console.log(key, keyPath);
    },
    handleClose(key, keyPath) {
      console.log(key, keyPath);
    },
    handleSelect(key) {
      if (key == 0) {
        this.isCollapse = !this.isCollapse;
      }
      this.collapseText = this.isCollapse ? "展开" : "收起";
    }
  }
};
</script>

<style>
</style>