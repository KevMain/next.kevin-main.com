import { createRouter, createWebHistory } from 'vue-router'
import Home from '../views/Home.vue'
import CV from '../views/CV.vue'
import Services from '../views/Services.vue'
import Projects from '../views/Projects.vue'
import Contact from '../views/Contact.vue'
import Running from '../views/Running.vue'
import Blog from '../views/Blog.vue'
import PostDetail from '../views/PostDetail.vue'

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home,
    meta: {
      title: 'Kevin Main — Senior .NET Software Engineer & Technical Lead',
      description: "Kevin Main is a Senior .NET Software Engineer & Technical Lead with over 20 years' experience in C#, .NET and software architecture, working with modern Azure development and AI-assisted engineering. Based in Cheshire, UK."
    }
  },
  {
    path: '/cv',
    name: 'CV',
    component: CV,
    meta: {
      title: 'Kevin Main CV — Senior .NET Software Engineer & Technical Lead',
      description: "CV of Kevin Main — Senior .NET Software Engineer & Technical Lead with over 20 years' experience in C#, .NET, ASP.NET Core, Azure and software architecture."
    }
  },
  {
    path: '/services',
    name: 'Services',
    component: Services,
    meta: {
      title: 'Services | Kevin Main — Senior .NET Software Engineer',
      description: 'Software engineering services from Kevin Main — .NET development, Azure cloud solutions, software architecture and technical leadership.'
    }
  },
  {
    path: '/projects',
    name: 'Projects',
    component: Projects,
    meta: {
      title: 'Software Engineering Projects — C#, .NET & Azure | Kevin Main',
      description: 'Software engineering projects by Kevin Main — C#, .NET, ASP.NET Core and Azure cloud-native development.'
    }
  },
  {
    path: '/contact',
    name: 'Contact',
    component: Contact,
    meta: {
      title: 'Contact Kevin Main — Senior .NET Software Engineer',
      description: 'Get in touch with Kevin Main — Senior .NET Software Engineer & Technical Lead based in Cheshire, UK.'
    }
  },
  {
    path: '/running',
    name: 'Running',
    component: Running,
    meta: {
      title: 'Running | Kevin Main',
      description: 'Running activities and training from Kevin Main.'
    }
  },
  {
    path: '/blog',
    name: 'Blog',
    component: Blog,
    meta: {
      title: 'Blog — .NET, Azure & AI-assisted Development | Kevin Main',
      description: 'Articles by Kevin Main on C#, .NET, Azure, software architecture and AI-assisted development.'
    }
  },
  {
    path: '/post/:slug',
    name: 'PostDetail',
    component: PostDetail,
    meta: {
      title: 'Blog | Kevin Main — Senior .NET Software Engineer',
      description: 'Articles by Kevin Main on C#, .NET, Azure, software architecture and AI-assisted development.'
    }
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes,
  scrollBehavior(to, from, savedPosition) {
    if (savedPosition) {
      return savedPosition
    } else {
      return { top: 0 }
    }
  }
})

const DEFAULT_TITLE = 'Kevin Main — Senior .NET Software Engineer & Technical Lead'

router.afterEach((to) => {
  document.title = to.meta.title || DEFAULT_TITLE

  if (to.meta.description) {
    let descriptionTag = document.querySelector('meta[name="description"]')
    if (!descriptionTag) {
      descriptionTag = document.createElement('meta')
      descriptionTag.setAttribute('name', 'description')
      document.head.appendChild(descriptionTag)
    }
    descriptionTag.setAttribute('content', to.meta.description)
  }
})

export default router
