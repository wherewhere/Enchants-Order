import { createI18n } from "vue-i18n";

export default createI18n({
    locale: (() => {
        const supportLanguages = ["en-US", "zh-CN"];
        const supportLanguageCodes =
            [
                ["en", "en-au", "en-ca", "en-gb", "en-ie", "en-in", "en-nz", "en-sg", "en-us", "en-za", "en-bz", "en-hk", "en-id", "en-jm", "en-kz", "en-mt", "en-my", "en-ph", "en-pk", "en-tt", "en-vn", "en-zw", "en-053", "en-021", "en-029", "en-011", "en-018", "en-014"],
                ["zh-Hans", "zh-cn", "zh-hans-cn", "zh-sg", "zh-hans-sg"]
            ];
        const fallbackLanguage = "en-US";
        const languages = navigator.languages || [navigator.language || fallbackLanguage];
        for (const lang of languages) {
            const temp = supportLanguageCodes.findIndex(codes => codes.some(x => x === lang.toLowerCase()));
            if (temp !== -1) {
                return supportLanguages[temp];
            }
        }
        return fallbackLanguage;
    })(),
    fallbackLocale: "en-US",
    messages: {
        "en-US": {
            title: "Enchants Order",
            description: "Get the best order for enchanting.",
            global: {
                lang: {
                    header: "Language",
                    description: "Select the language you want to use.",
                    select: {
                        placeholder: "Choose Language"
                    }
                }
            },
            object: {
                header: "Target Item",
                button: {
                    title: "Get Results"
                },
                switch: {
                    icon: {
                        title: "Presets"
                    },
                    header: "Use Presets",
                    description: "Switch if using preset object or not.",
                    switch: {
                        on: "On",
                        off: "Off"
                    }
                },
                penalty: {
                    icon: {
                        title: "Penalty"
                    },
                    header: "Initial Penalty",
                    description: "Set the initial penalty of the item witch you want to enchant."
                },
                select: {
                    icon: {
                        title: "Item"
                    },
                    header: "Select Item",
                    description: "Select the target item by name.",
                    select: {
                        placeholder: "Choose Item"
                    }
                }
            },
            enchantment: {
                header: "Add Enchantment",
                button: {
                    title: "Add"
                },
                name: {
                    icon: {
                        title: "Name"
                    },
                    header: "Enchantment",
                    description: "Select the target enchantment by name.",
                    combobox: {
                        placeholder: "Choose Enchantment"
                    }
                },
                level: {
                    header: "Level",
                    description: "Set the level of the target enchantment."
                },
                weight: {
                    header: "Weight",
                    description: "Set the weight of the target enchantment. <b>Do not change</b> if you are not sure what it is."
                }
            },
            list: {
                header: "Enchantment List",
                button: {
                    title: "Order"
                },
                item: {
                    icon: {
                        title: "Enchantment"
                    },
                    description: "Level: {0} {'|'} Weight: {1} {'|'} Experience: {2}",
                    button: {
                        title: "Delete"
                    }
                }
            },
            results: {
                header: "Results",
                step: {
                    icon: {
                        title: "Path"
                    },
                    header: "Total Experience Level: {0}",
                    description: "Penalty Level: {0} {'|'} Max Experience Level: {1}",
                    content: "Step {0}:"
                }
            },
        },
        "zh-CN": {
            title: "附魔排序",
            description: "获取最佳附魔顺序。",
            global: {
                lang: {
                    header: "语言",
                    description: "选择您想使用的语言。",
                    combobox: {
                        placeholder: "选择语言"
                    }
                }
            },
            object: {
                header: "目标物品",
                button: {
                    title: "获取结果"
                },
                switch: {
                    icon: {
                        title: "预设"
                    },
                    header: "使用预设",
                    description: "切换是否使用预设物品。",
                    switch: {
                        on: "开",
                        off: "关"
                    }
                },
                penalty: {
                    icon: {
                        title: "惩罚"
                    },
                    header: "初始惩罚",
                    description: "设置您想要附魔的物品的初始惩罚。"
                },
                select: {
                    icon: {
                        title: "物品"
                    },
                    header: "选择物品",
                    description: "通过名称选择目标物品。",
                    select: {
                        placeholder: "选择物品"
                    }
                }
            },
            enchantment: {
                header: "添加附魔",
                button: {
                    title: "添加"
                },
                name: {
                    icon: {
                        title: "名称"
                    },
                    header: "附魔",
                    description: "通过名称选择目标附魔。",
                    combobox: {
                        placeholder: "选择附魔"
                    }
                },
                level: {
                    header: "等级",
                    description: "设置目标附魔的等级。"
                },
                weight: {
                    header: "权重",
                    description: "设置目标附魔的权重。如果您不确定它是什么，请<b>不要更改</b>。"
                }
            },
            list: {
                header: "附魔列表",
                button: {
                    title: "排序"
                },
                item: {
                    icon: {
                        title: "附魔"
                    },
                    description: "等级: {0} {'|'} 权重: {1} {'|'} 经验: {2}",
                    button: {
                        title: "移除"
                    }
                }
            },
            results: {
                header: "结果",
                step: {
                    icon: {
                        title: "路径"
                    },
                    header: "总经验等级: {0}",
                    description: "惩罚等级: {0} {'|'} 最高所需等级: {1}",
                    content: "步骤 {0}:"
                }
            }
        }
    }
});